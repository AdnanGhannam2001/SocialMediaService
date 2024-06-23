using System.Diagnostics;
using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Enums;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetPostsPage;

public sealed class GetPostsPageHandler : IRequestHandler<GetPostsPageQuery, Result<Page<Post>>>
{
    private readonly IProfileRepository _profileRepo;
    private readonly IPostRepository _postRepo;

    public GetPostsPageHandler(IProfileRepository profileRepo,
        IPostRepository postRepo)
    {
        _profileRepo = profileRepo;
        _postRepo = postRepo;
    }

    public async Task<Result<Page<Post>>> Handle(GetPostsPageQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var page = request.Type switch
        {
            PostsTypes.FollowedPosts => await _postRepo.GetFollowedPostsPageAsync(request.ProfileId, request.Request, cancellationToken),
            PostsTypes.FriendsPosts => await _postRepo.GetFriendsPostsPageAsync(request.ProfileId, request.Request, cancellationToken),
            PostsTypes.SavedPosts => await _postRepo.GetSavedPostsPageAsync(request.ProfileId, request.Request, cancellationToken),
            PostsTypes.HiddenPosts => await _postRepo.GetHiddenPageAsync(profile.Id, request.Request, cancellationToken),
            _ => throw new UnreachableException()
        };

        return page;
    }
}
