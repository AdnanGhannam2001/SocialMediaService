using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Queries.GetGroup;

public sealed record GetGroupQuery(string GroupId, string? RequesterId) : IQuery<Group>;