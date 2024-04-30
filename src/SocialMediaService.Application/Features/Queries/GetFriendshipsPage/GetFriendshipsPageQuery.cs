using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetFriendshipsPage;

public sealed record GetFriendshipsPageQuery(string ProfileId, PageRequest<Friendship> Request, string? RequesterId = null) : IQuery<Page<Friendship>>;