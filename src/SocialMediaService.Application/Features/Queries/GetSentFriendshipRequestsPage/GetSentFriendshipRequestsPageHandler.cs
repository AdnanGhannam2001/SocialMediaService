using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetSentFriendshipRequestsPage;

public sealed class GetSentFriendshipRequestsPageHandler
    : IRequestHandler<GetSentFriendshipRequestsPageQuery, Result<Page<FriendshipRequest>>>
{
    private readonly IProfileRepository _repo;

    public GetSentFriendshipRequestsPageHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Page<FriendshipRequest>>> Handle(GetSentFriendshipRequestsPageQuery request, CancellationToken cancellationToken)
    {
        var friendshipRequests = await _repo.GetSentFriendshipRequestsPageAsync(request.UserId, request.Request, cancellationToken);

        return friendshipRequests;
    }
}
