using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Queries.GetInvites;

public sealed record GetInvitesPageQuery(string ProfileId, PageRequest<Invite> Request) : IQuery<Page<Invite>>;