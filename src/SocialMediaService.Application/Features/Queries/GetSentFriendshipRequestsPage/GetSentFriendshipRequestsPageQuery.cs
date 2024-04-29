using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetSentFriendshipRequestsPage;

public sealed record GetSentFriendshipRequestsPageQuery(string UserId) : IQuery<Page<FriendshipRequest>>;