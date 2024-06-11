using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Queries.GetGroupsPageFor;

public sealed record GetGroupsPageForQuery(string ProfileId, PageRequest<Group> Request, string? RequesterId = null) : IQuery<Page<Group>>;