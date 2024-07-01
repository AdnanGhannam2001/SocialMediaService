using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetProfilesNamesByIds;

public sealed record GetProfilesNamesByIdsQuery(IEnumerable<string> Ids) : IQuery<IEnumerable<GetProfilesNamesByIdsResult>>;