using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetFriendshipRequestsPage;

public sealed record GetFriendshipRequestsPageQuery(string ProfileId, bool Sent, PageRequest<FriendshipRequest> Request)
    : IQuery<Page<FriendshipRequest>>;