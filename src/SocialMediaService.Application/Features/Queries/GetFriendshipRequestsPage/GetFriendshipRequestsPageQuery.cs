using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetFriendshipRequestsPage;

public sealed record GetFriendshipRequestsPageQuery(string UserId, PageRequest<FriendshipRequest> Request) : IQuery<Page<FriendshipRequest>>;