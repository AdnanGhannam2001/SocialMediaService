using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Queries.GetGroupPostsPage;

public sealed record GetGroupPostsPageQuery(string GroupId, PageRequest<Post> Request, string? RequesterId = null) : IQuery<Page<Post>>;