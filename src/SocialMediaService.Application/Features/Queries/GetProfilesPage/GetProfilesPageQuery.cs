using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetProfilesPage;

public sealed record GetProfilesPageQuery(PageRequest<Profile> Request) : IQuery<Page<Profile>>;