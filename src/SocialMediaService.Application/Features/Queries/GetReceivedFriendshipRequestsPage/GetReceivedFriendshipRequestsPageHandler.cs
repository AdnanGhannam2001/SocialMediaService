using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetReceivedFriendshipRequestsPage;

public sealed class GetReceivedFriendshipRequestsPageHandler : IRequestHandler<GetReceivedFriendshipRequestsPageQuery, Result<Page<FriendshipRequest>>>
{
    private readonly IProfileRepository _repo;

    public GetReceivedFriendshipRequestsPageHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Page<FriendshipRequest>>> Handle(GetReceivedFriendshipRequestsPageQuery request, CancellationToken cancellationToken)
    {
        var friendshipRequests = await _repo.GetReceivedFriendshipRequestsPageAsync(request.UserId, request.Request, cancellationToken);

        return friendshipRequests;
    }
}
