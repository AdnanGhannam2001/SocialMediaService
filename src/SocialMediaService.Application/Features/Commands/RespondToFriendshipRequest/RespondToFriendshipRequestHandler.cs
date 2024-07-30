using static System.Diagnostics.Debug;
using MediatR;
using Microsoft.Extensions.Logging;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;
using MassTransit;
using PR2.Contracts.Events;

namespace SocialMediaService.Application.Features.Commands.RespondToFriendshipRequest;

public sealed class RespondToFriendshipRequestHandler : IRequestHandler<RespondToFriendshipRequestCommand, Result<Unit>>
{
    private readonly ILogger<RespondToFriendshipRequestHandler> _logger;
    private readonly IProfileRepository _repo;
    private readonly IPublishEndpoint _publisher;

    public RespondToFriendshipRequestHandler(ILogger<RespondToFriendshipRequestHandler> logger,
        IProfileRepository repo,
        IPublishEndpoint publisher)
    {
        _logger = logger;
        _repo = repo;
        _publisher = publisher;
    }

    public async Task<Result<Unit>> Handle(RespondToFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        var sender = await _repo.GetWithFriendshipRequestAsync(request.SenderId, request.ReceiverId, cancellationToken);

        if (sender is null)
        {
            return new RecordNotFoundException("Sender profile is not found");
        }

        if (sender.SentRequests.Count == 0)
        {
            return new RecordNotFoundException("You didn't received a friendship request from this user");
        }

        using var transaction = await _repo.BeginTransactionAsync();

        try
        {
            Friendship? friendship = null;
            var receiver = await _repo.GetByIdAsync(request.ReceiverId, cancellationToken);
            Assert(receiver is not null);

            // Add Friendship if Accepted
            if (request.Accept)
            {
                friendship = new Friendship(sender, receiver);
                sender.AddFriend(friendship);

                var message = new FriendshipCreatedEvent(receiver.Id, sender.Id);
                await _publisher.Publish(message, cancellationToken);
            }

            sender.RemoveFriendshipRequest(sender.SentRequests.ElementAt(0));

            await _repo.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            var notification = new NotifyEvent(sender.Id,
                $"{receiver.FirstName} " + (request.Accept ? "accepted" : "rejected") + " your friendship request",
                $"profiles/{receiver.Id}");
            await _publisher.Publish(notification, cancellationToken);

            return Unit.Value;
        }
        catch (Exception exp)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Error happend while trying to handle {0}: {1}", nameof(RespondToFriendshipRequestHandler), exp.Message);
            return new TransactionFailureException("Something went wrong");
        }
    }
}
