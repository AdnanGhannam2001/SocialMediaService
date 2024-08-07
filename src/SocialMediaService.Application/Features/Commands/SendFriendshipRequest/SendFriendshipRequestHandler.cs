using static System.Diagnostics.Debug;
using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;
using MassTransit;
using PR2.Contracts.Events;

namespace SocialMediaService.Application.Features.Commands.SendFriendshipRequest;

public sealed class SendFriendshipRequestHandler : IRequestHandler<SendFriendshipRequestCommand, Result<FriendshipRequest>>
{
    private readonly IProfileRepository _repo;
    private readonly IPublishEndpoint _publisher;

    public SendFriendshipRequestHandler(IProfileRepository repo,
        IPublishEndpoint publisher)
    {
        _repo = repo;
        _publisher = publisher;
    }

    public async  Task<Result<FriendshipRequest>> Handle(SendFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        if (await ProfileHelper.IsBlocked(_repo, request.SenderId, request.ReceiverId, cancellationToken))
        {
            return new RecordNotFoundException($"Profile is not found");
        }

        // Check for Existed Friendship
        {
            var sender = await _repo.GetWithFriendshipAsync(request.SenderId, request.ReceiverId, cancellationToken);

            if (sender is not null && sender.SentRequests.Count > 0)
            {
                return new DuplicatedRecordException("You're already a friend with this user");
            }
        }

        // Send Friendship Request
        {
            var sender = await _repo.GetWithFriendshipRequestAsync(request.SenderId, request.ReceiverId, cancellationToken);

            if (sender is null)
            {
                return new RecordNotFoundException("Sender profile is not found");
            }

            if (sender.SentRequests.Count > 0)
            {
                return new DuplicatedRecordException("You already sent a request to this user");
            }

            var receiver = await _repo.GetByIdAsync(request.ReceiverId, cancellationToken);

            Assert(receiver is not null);

            var friendshipRequest = new FriendshipRequest(sender, receiver);
            sender.AddFriendshipRequest(friendshipRequest);
            await _repo.SaveChangesAsync(cancellationToken);

            var notification = new NotifyEvent(receiver.Id,
                $"You got a friendship request from {sender.FirstName}",
                $"sender/{sender.Id}");
            await _publisher.Publish(notification, cancellationToken);

            return friendshipRequest;
        }
    }
}
