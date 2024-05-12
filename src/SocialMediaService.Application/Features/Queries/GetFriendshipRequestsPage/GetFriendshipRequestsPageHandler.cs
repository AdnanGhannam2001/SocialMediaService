using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetFriendshipRequestsPage;

public sealed class GetFriendshipRequestsPageHandler : IRequestHandler<GetFriendshipRequestsPageQuery, Result<Page<FriendshipRequest>>>
{
    private readonly IProfileRepository _repo;

    public GetFriendshipRequestsPageHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Page<FriendshipRequest>>> Handle(GetFriendshipRequestsPageQuery request, CancellationToken cancellationToken)
    {
        var friendshipRequests = request.Sent
            ? await _repo.GetSentFriendshipRequestsPageAsync(request.ProfileId, request.Request, cancellationToken)
            : await _repo.GetReceivedFriendshipRequestsPageAsync(request.ProfileId, request.Request, cancellationToken);

        return friendshipRequests;
    }
}
