using MassTransit;
using MediatR;
using PR2.Contracts.Events;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.DeleteMember;

public sealed class DeleteMemberHandler : IRequestHandler<DeleteMemberCommand, Result<Member>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;
    private readonly IPublishEndpoint _publisher;

    public DeleteMemberHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo,
        IPublishEndpoint publisher)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
        _publisher = publisher;
    }

    public async Task<Result<Member>> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var memberProfile = await _profileRepo.GetByIdAsync(request.MemberId, cancellationToken);

        if (memberProfile is null)
        {
            return new RecordNotFoundException("Member Profile is not found");
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

        var member = group.Members.ElementAt(0);

        // Check if profile is organizer
        if (await _groupRepo.CountMembersAsync(group.Id,
            x => (x.Role <= MemberRoleTypes.Organizer) && x.ProfileId.Equals(request.ProfileId),
            cancellationToken) == 0)
        {
            return new UnauthorizedException("You have to be an organizer or higher to perform this action");
        }
        
        using var transaction = await _groupRepo.BeginTransactionAsync();

        try
        {
            if (request.Kick)
            {
                var kicked = new Kicked(group, memberProfile, profile, request.Reason);
                group.Kick(kicked);
            }

            group.RemoveMember(member);
            await _groupRepo.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            if (request.Kick)
            {
                var notification = new NotifyEvent(member.ProfileId,
                    $"You got kicked out of '{group.Name}' group, because: \"{request.Reason}\"",
                    $"groups/{group.Id}");
                await _publisher.Publish(notification, cancellationToken);
            }

            var message = new MemberLeavedEvent(group.Id, member.ProfileId);
            await _publisher.Publish(message, cancellationToken);

            return member;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return new TransactionFailureException("Failed to kick member");
        }
    }
}
