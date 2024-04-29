using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetFriendshipsPage;

public sealed record GetFriendshipsPageQuery(string UserId, PageRequest<Friendship> Request) : IQuery<Page<Friendship>>;