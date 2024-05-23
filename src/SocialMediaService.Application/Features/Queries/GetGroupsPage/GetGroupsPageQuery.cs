using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Queries.GetGroupsPage;

public sealed record GetGroupsPageQuery(PageRequest<Group> Request) : IQuery<Page<Group>>;