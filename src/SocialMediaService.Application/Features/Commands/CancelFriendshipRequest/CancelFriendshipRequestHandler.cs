using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.CancelFriendshipRequest;

public sealed class CancelFriendshipRequestHandler : IRequestHandler<CancelFriendshipRequestCommand, Result<FriendshipRequest>>
{
    private readonly IProfileRepository _repo;

    public CancelFriendshipRequestHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<FriendshipRequest>> Handle(CancelFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        var friendshipRequest = await _repo.GetFriendshipRequestAsync(request.SenderId, request.ReceiverId, cancellationToken);

        if (friendshipRequest is null)
        {
            return new RecordNotFoundException("Friendship request is not found");
        }

        await _repo.DeleteFriendshipRequestAsync(friendshipRequest, cancellationToken);

        return friendshipRequest;
    }
}
