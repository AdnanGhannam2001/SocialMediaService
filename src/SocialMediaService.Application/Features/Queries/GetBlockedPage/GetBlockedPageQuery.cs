using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetBlockedPage;

public sealed record GetBlockedPageQuery(string ProfileId, PageRequest<Block> Request) : IQuery<Page<Block>>;