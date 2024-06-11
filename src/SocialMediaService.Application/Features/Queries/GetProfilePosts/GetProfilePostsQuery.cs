using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Queries.GetProfilePosts;

public sealed record GetProfilePostsQuery(string ProfileId, PageRequest<Post> Request, string? RequesterId = null) : IQuery<Page<Post>>;