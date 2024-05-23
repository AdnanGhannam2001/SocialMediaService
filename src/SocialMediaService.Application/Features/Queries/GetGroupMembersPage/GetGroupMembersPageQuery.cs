using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Queries.GetGroupMembersPage;

public sealed record GetGroupMembersPageQuery(string GroupId, string? RequesterId, PageRequest<Member> Request) : IQuery<Page<Member>>;