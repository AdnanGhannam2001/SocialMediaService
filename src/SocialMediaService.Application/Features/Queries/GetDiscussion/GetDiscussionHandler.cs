using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetDiscussion;

public sealed class GetDiscussionHandler : IRequestHandler<GetDiscussionQuery, Result<Discussion>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IGroupRepository _groupRepo;

    public GetDiscussionHandler(IProfileRepository profileRepo,
        IGroupRepository groupRepo)
    {
        _profileRepo = profileRepo;
        _groupRepo = groupRepo;
    }

    public async Task<Result<Discussion>> Handle(GetDiscussionQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var group = await _groupRepo.GetWithDiscussionAsync(request.GroupId, request.ProfileId, cancellationToken);

        if (group is null
            || (group.Visibility == GroupVisibilities.Hidden
                && await _groupRepo.CountMembersAsync(group.Id, x => x.ProfileId.Equals(profile.Id) && x.GroupId.Equals(group.Id), cancellationToken) == 0))
        {
            return new RecordNotFoundException("Group is not found");
        }

        if (group.Discussions.Count == 0)
        {
            return new RecordNotFoundException("Discussion is not found");
        }

        return group.Discussions.ElementAt(0);
    }
}
