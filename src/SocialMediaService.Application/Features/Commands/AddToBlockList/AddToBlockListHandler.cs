using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.AddToBlockList;

public sealed class AddToBlockListHandler : IRequestHandler<AddToBlockListCommand, Result<Block>>
{
    private readonly IProfileRepository _repo;

    public AddToBlockListHandler(IProfileRepository repo)
    {
        _repo = repo;
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

            if (p!.Friends.Count > 0) p.RemoveFriend(p.Friends.ElementAt(0));
        }

        // Delete Follow
        {
            var p = await _repo.GetWithFollowedAsync(request.BlockerId, request.ProfileId, cancellationToken);
            if (p!.Following.Count > 0) p.RemoveFollow(p.Following.ElementAt(0));
        }
        {
            var p = await _repo.GetWithFollowedAsync(request.ProfileId, request.BlockerId, cancellationToken);
            if (p!.Following.Count > 0) p.RemoveFollow(p.Following.ElementAt(0));
        }

        var block = new Block(blocker, profile, request.Reason);
        blocker.AddBlocked(block);

        await _repo.SaveChangesAsync(cancellationToken);

        return block;
    }
}
