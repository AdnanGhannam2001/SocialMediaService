using SocialMediaService.Application.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetGroupsNamesByIds;

public sealed record GetGroupsNamesByIdsQuery(IEnumerable<string> Ids) : IQuery<IEnumerable<GetGroupsNamesByIdsResult>>;