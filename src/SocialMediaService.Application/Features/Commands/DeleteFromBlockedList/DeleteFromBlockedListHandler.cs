using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.DeleteFromBlockedList;

public sealed class DeleteFromBlockedListHandler : IRequestHandler<DeleteFromBlockedListCommand, Result<Block>>
{
    private readonly IProfileRepository _repo;

    public DeleteFromBlockedListHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Block>> Handle(DeleteFromBlockedListCommand request, CancellationToken cancellationToken)
    {
        var blocker = await _repo.GetWithBlockedAsync(request.BlockerId, request.BlockedId, cancellationToken);

        if (blocker is null)
        {
            return new RecordNotFoundException("Blocker profile is not found");
        }

        if (blocker.Blocked.Count == 0)
        {
            return new RecordNotFoundException("This profile is not blocked");
        }

        var blocked = blocker.Blocked.ElementAt(0);

        using var transaction = await _repo.BeginTransactionAsync();

        try
        {
            blocker.RemoveBlocked(blocked);
            await _repo.SaveChangesAsync(cancellationToken);

            // FIXME
            // // Delete Sent Friendship Request
            // {
            //     var friendshipRequest = await _repo.GetFriendshipRequestAsync(request.BlockerId, request.BlockedId, cancellationToken);
            //     if (friendshipRequest is not null) await _repo.DeleteFriendshipRequestAsync(friendshipRequest, cancellationToken);
            // }
            // // Delete Received Friendship Request
            // {
            //     var friendshipRequest = await _repo.GetFriendshipRequestAsync(request.BlockedId, request.BlockerId, cancellationToken);
            //     if (friendshipRequest is not null) await _repo.DeleteFriendshipRequestAsync(friendshipRequest, cancellationToken);
            // }

            // // Delete Friendship
            // {
            //     var friendship = await _repo.GetFriendshipAsync(request.BlockedId, request.BlockerId, cancellationToken);
            //     if (friendship is not null) await _repo.DeleteFriendshipAsync(friendship, cancellationToken);
            // }

            // // Delete Followed
            // {
            //     var follow = await _repo.GetFollowedAsync(request.BlockerId, request.BlockedId, cancellationToken);
            //     if (follow is not null) await _repo.DeleteFollowAsync(follow, cancellationToken);
            // }

            // // Delete Follower
            // {
            //     var follow = await _repo.GetFollowedAsync(request.BlockedId, request.BlockerId, cancellationToken);
            //     if (follow is not null) await _repo.DeleteFollowAsync(follow, cancellationToken);
            // }

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }

        return blocked;
    }
}
