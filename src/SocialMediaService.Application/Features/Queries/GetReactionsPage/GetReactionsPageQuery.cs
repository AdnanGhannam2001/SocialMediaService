using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Queries.GetReactionsPage;

public sealed record GetReactionsPageQuery(string PostId, PageRequest<Reaction> Request, string? ProfileId = null) : IQuery<Page<Reaction>>;