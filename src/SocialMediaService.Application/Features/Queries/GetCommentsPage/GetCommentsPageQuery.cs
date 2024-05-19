using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Queries.GetCommentsPage;

public sealed record GetCommentsPageQuery(string PostId, string? ParentId, string? ProfileId, PageRequest<Comment> Request) : IQuery<Page<Comment>>;