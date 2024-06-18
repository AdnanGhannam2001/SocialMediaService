using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetFollowingPage;

public sealed record GetFollowingPageQuery(string UserId, PageRequest<Follow> Request) : IQuery<Page<Follow>>;