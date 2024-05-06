using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetFollowsPage;

public sealed record GetFollowsPageQuery(string UserId, PageRequest<Follow> Request) : IQuery<Page<Follow>>;