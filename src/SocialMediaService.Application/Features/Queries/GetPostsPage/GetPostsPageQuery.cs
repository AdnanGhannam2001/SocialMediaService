using PR2.Shared.Common;
using SocialMediaService.Application.Enums;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Queries.GetPostsPage;

public sealed record GetPostsPageQuery(string ProfileId, PageRequest<Post> Request, PostsTypes Type = PostsTypes.FollowedPosts)
    : IQuery<Page<Post>>;