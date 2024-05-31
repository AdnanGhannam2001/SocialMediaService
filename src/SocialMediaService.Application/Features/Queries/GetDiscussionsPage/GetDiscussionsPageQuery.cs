using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Queries.GetDiscussionsPage;

public sealed record GetDiscussionsPageQuery(string GroupId, PageRequest<Discussion> Request, string? ProfileId = null)
    : IQuery<Page<Discussion>>;