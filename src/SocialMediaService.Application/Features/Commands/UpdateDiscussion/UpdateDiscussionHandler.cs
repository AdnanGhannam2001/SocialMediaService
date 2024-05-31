using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.UpdateDiscussion;

public sealed class UpdateDiscussionHandler : IRequestHandler<UpdateDiscussionCommand, Result<Discussion>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public UpdateDiscussionHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Discussion>> Handle(UpdateDiscussionCommand request, CancellationToken cancellationToken)
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

        if (discussion.ProfileId != request.ProfileId)
        {
            return new UnauthorizedException("Only the publisher can modify this");
        }

        discussion.Update(request.Title, request.Content, request.Tags);
        await _groupRepo.SaveChangesAsync(cancellationToken);
        
        return discussion;
    }
}
