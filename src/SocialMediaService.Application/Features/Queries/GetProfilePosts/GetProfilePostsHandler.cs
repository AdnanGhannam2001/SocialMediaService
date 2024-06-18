using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetProfilePosts;

public sealed class GetProfilePostsHandler : IRequestHandler<GetProfilePostsQuery, Result<Page<Post>>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public GetProfilePostsHandler(IProfileRepository profileRepo,
        IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Page<Post>>> Handle(GetProfilePostsQuery request, CancellationToken cancellationToken)
    {
        if (request.RequesterId is not null &&
            await ProfileHelper.IsBlocked(_profileRepo, request.ProfileId, request.RequesterId, cancellationToken))
        {
            return new RecordNotFoundException($"Profile is not found");
        }

        var visibility = request.RequesterId == null
            ? PostVisibilities.Public
            : request.RequesterId == request.ProfileId
                ? PostVisibilities.Private
                : await _profileRepo.CountFriendshipsAsync(request.ProfileId, x => x.FriendId.Equals(request.RequesterId), cancellationToken) > 0
                    ? PostVisibilities.Friends
                    : PostVisibilities.Public;

        var page = await _postRepo.GetProfilePostsPageAsync(request.ProfileId, request.Request, visibility, cancellationToken);

        return page;
    }
}
