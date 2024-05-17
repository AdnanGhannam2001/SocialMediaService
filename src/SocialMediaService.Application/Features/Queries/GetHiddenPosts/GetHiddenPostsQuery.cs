using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Queries.GetHiddenPosts;

public sealed record GetHiddenPostsQuery(string ProfileId, PageRequest<Post> Request) : IQuery<Page<Post>>;