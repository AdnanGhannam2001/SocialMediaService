using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetReceivedFriendshipRequestsPage;

public sealed record GetReceivedFriendshipRequestsPageQuery(string UserId) : IQuery<Page<FriendshipRequest>>;