using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Queries.GetJoinRequestsPage;

public sealed record GetJoinRequestsPageQuery(string GroupId, string RequesterId, PageRequest<JoinRequest> Request) : IQuery<Page<JoinRequest>>;