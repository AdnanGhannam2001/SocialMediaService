using MassTransit;
using MediatR;
using PR2.Contracts.Events;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.ChangeMemberRole;

public sealed class ChangeMemberRoleHandler : IRequestHandler<ChangeMemberRoleCommand, Result<Member>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;
    private readonly IPublishEndpoint _publisher;

    public ChangeMemberRoleHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo,
        IPublishEndpoint publisher)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
        _publisher = publisher;
    }

    public async Task<Result<Member>> Handle(ChangeMemberRoleCommand request, CancellationToken cancellationToken)
    {
        var memberProfile = await _profileRepo.GetByIdAsync(request.MemberId, cancellationToken);

        if (memberProfile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, memberProfile.Id, cancellationToken);

        if (group is null)
        {
            return new RecordNotFoundException("Group is not found");
        }

        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        // Check if profile is admin
        if (await _groupRepo.CountMembersAsync(group.Id,
            x => x.Role == MemberRoleTypes.Admin && x.ProfileId.Equals(request.ProfileId),
            cancellationToken) == 0)
        {
            return new UnauthorizedException("You have to be an admin to perform this action");
        }

        var member = group.Members.ElementAt(0);
        member.ChangeRole(request.Role);
        await _groupRepo.SaveChangesAsync(cancellationToken);

        var notification = new NotifyEvent(member.ProfileId,
            $"Your role has been changed to '{member.Role}' in '{group.Name}' group",
            $"groups/{group.Id}");
        await _publisher.Publish(notification, cancellationToken);

        var message = new MemberRoleChangedEvent(group.Id, member.ProfileId, member.Role.ToString());
        await _publisher.Publish(message, cancellationToken);

        return member;
    }
}
