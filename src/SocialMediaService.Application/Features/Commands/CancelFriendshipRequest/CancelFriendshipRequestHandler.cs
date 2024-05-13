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
        var sender = await _repo.GetWithFriendshipRequestAsync(request.SenderId, request.ReceiverId, cancellationToken);

        if (sender is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        if (sender.SentRequests.Count == 0)
        {
            return new RecordNotFoundException("Friendship request is not found");
        }

        var friendshipRequest = sender.SentRequests.ElementAt(0);

        sender.RemoveFriendshipRequest(friendshipRequest);
        await _repo.SaveChangesAsync(cancellationToken);
        
        return friendshipRequest;
    }
}
