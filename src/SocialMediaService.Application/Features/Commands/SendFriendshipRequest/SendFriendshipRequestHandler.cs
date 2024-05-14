using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.SendFriendshipRequest;

public sealed class SendFriendshipRequestHandler : IRequestHandler<SendFriendshipRequestCommand, Result<FriendshipRequest>>
{
    private readonly IProfileRepository _repo;

    public SendFriendshipRequestHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async  Task<Result<FriendshipRequest>> Handle(SendFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        if (await ProfileHelper.IsBlocked(_repo, request.SenderId, request.ReceiverId, cancellationToken))
        {
            return new RecordNotFoundException($"Profile is not found");
        }

        // FIXME
        // Check for Existed Friendship
        // {
        //     var friendship = await _repo.GetFriendshipAsync(request.SenderId, request.ReceiverId, cancellationToken);

        //     if (friendship is not null)
        //     {
        //         return new DuplicatedRecordException("You're already a friend with this user");
        //     }
        // }

        var sender = await _repo.GetWithFriendshipRequestAsync(request.SenderId, request.ReceiverId, cancellationToken);

        ArgumentNullException.ThrowIfNull(sender);

        if (sender.SentRequests.Count > 0)
        {
            return new DuplicatedRecordException("You already sent a request to this user");
        }

        var receiver = await _repo.GetByIdAsync(request.ReceiverId, cancellationToken);

        ArgumentNullException.ThrowIfNull(receiver);

        var friendshipRequest = new FriendshipRequest(sender, receiver);
        sender.AddFriendshipRequest(friendshipRequest);
        await _repo.SaveChangesAsync(cancellationToken);

        return friendshipRequest;
    }
}
