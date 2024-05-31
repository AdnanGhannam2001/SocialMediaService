using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Queries.GetKickedPage;

public sealed record GetKickedPageQuery(string GroupId, string ProfileId, PageRequest<Kicked> Request) : IQuery<Page<Kicked>>;