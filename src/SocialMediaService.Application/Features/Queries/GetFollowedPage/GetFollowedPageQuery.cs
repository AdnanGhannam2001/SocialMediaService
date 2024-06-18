using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetFollowedPage;

public sealed record GetFollowedPageQuery(string UserId, PageRequest<Follow> Request) : IQuery<Page<Follow>>;