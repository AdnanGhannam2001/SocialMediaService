using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Enums;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.DeleteDiscussion;

public sealed class DeleteDiscussionHandler : IRequestHandler<DeleteDiscussionCommand, Result<Discussion>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public DeleteDiscussionHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Discussion>> Handle(DeleteDiscussionCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var group = await _groupRepo.GetWithDiscussionAsync(request.GroupId, request.ProfileId, cancellationToken);

        if (group is null)
        {
            return new RecordNotFoundException("Group is not found");
        }

        var isMember = await _groupRepo.CountMembersAsync(group.Id, x => x.ProfileId.Equals(profile.Id) && x.GroupId.Equals(group.Id), cancellationToken) > 0;

        if (!isMember)
        {
            if (group.Visibility == GroupVisibilities.Hidden) return new RecordNotFoundException("Group is not found");
            if (group.Visibility == GroupVisibilities.Private) return new UnauthorizedException("This group is private");
        }

        if (group.Discussions.Count == 0)
        {
            return new RecordNotFoundException("Discussion is not found");
        }

        var discussion = group.Discussions.ElementAt(0);

        var isAuthorized = await _groupRepo.CountMembersAsync(group.Id,
            x => x.ProfileId.Equals(profile.Id) && x.GroupId.Equals(group.Id) && x.Role != MemberRoleTypes.Normal,
            cancellationToken) > 0;

        if (discussion.ProfileId != request.ProfileId && !isAuthorized)
        {
            return new UnauthorizedException("Only the publisher, organizers and admins can delete this");
        }

        group.RemoveDiscussion(discussion);
        await _groupRepo.SaveChangesAsync(cancellationToken);
        
        return discussion;
    }
}
