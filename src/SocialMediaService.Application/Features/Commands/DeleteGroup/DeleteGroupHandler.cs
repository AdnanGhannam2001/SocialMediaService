using MassTransit;
using MediatR;
using PR2.Contracts.Events;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.DeleteGroup;

public sealed class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, Result<Group>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;
    private readonly IPublishEndpoint _publisher;

    public DeleteGroupHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo,
        IPublishEndpoint publisher)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
        _publisher = publisher;
    }

    public async Task<Result<Group>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var group = await _groupRepo.GetWithMemebershipAsync(request.GroupId, profile.Id, cancellationToken);

        if (group is null)
        {
            return new RecordNotFoundException("Group is not found");
        }

        if (group.Members.Count == 0 || group.Members.ElementAt(0).Role != MemberRoleTypes.Admin)
        {
            return new UnauthorizedException("Only Admins in this group can perform this action");
        }

        await _groupRepo.DeleteAsync(group, cancellationToken);

        var message = new GroupDeletedEvent(group.Id);
        await _publisher.Publish(message, cancellationToken);

        return group;
    }
}
