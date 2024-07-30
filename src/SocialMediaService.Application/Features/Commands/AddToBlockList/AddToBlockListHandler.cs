using MassTransit;
using MediatR;
using PR2.Contracts.Events;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.AddToBlockList;

public sealed class AddToBlockListHandler : IRequestHandler<AddToBlockListCommand, Result<Block>>
{
    private readonly IProfileRepository _repo;
    private readonly IPublishEndpoint _publisher;

    public AddToBlockListHandler(IProfileRepository repo,
        IPublishEndpoint publisher)
    {
        _repo = repo;
        _publisher = publisher;
    }

    public async Task<Result<Block>> Handle(AddToBlockListCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (await ProfileHelper.IsBlocked(_repo, request.BlockerId, request.ProfileId, cancellationToken)
            || profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var blocker = await _repo.GetWithBlockedAsync(request.BlockerId, request.ProfileId, cancellationToken);

        if (blocker is null)
        {
            return new RecordNotFoundException("Blocker profile is not found");
        }

        if (blocker.Blocked.Count > 0)
        {
            return new DuplicatedRecordException("Profile is already blocked");
        }

        // Delete Friendship Request
        {
            var p = await _repo.GetWithFriendshipRequestAsync(request.BlockerId, request.ProfileId, cancellationToken)
                ?? await _repo.GetWithFriendshipRequestAsync(request.ProfileId, request.BlockerId, cancellationToken);

            if (p!.Blocked.Count > 0) p.RemoveFriendshipRequest(p.SentRequests.ElementAt(0));
        }

        // Delete Friendship
        {
            var p = await _repo.GetWithFriendshipAsync(request.ProfileId, request.BlockerId, cancellationToken)
                ?? await _repo.GetWithFriendshipAsync(request.BlockerId, request.ProfileId, cancellationToken);

            if (p!.Friends.Count > 0)
            {
                var friendship = p!.Friends.ElementAt(0);
                p.RemoveFriend(friendship);
                var message = new FriendshipDeletedEvent(friendship.ProfileId, friendship.FriendId);
                await _publisher.Publish(message, cancellationToken);
            }
        }

        // Delete Follow
        {
            var p = await _repo.GetWithFollowingAsync(request.BlockerId, request.ProfileId, cancellationToken);
            if (p!.Following.Count > 0) p.RemoveFollow(p.Following.ElementAt(0));
        }
        {
            var p = await _repo.GetWithFollowingAsync(request.ProfileId, request.BlockerId, cancellationToken);
            if (p!.Following.Count > 0) p.RemoveFollow(p.Following.ElementAt(0));
        }

        // TODO: Make Block.Reason Optional
        var block = new Block(blocker, profile, request.Reason ?? "");
        blocker.AddBlocked(block);

        await _repo.SaveChangesAsync(cancellationToken);

        return block;
    }
}
