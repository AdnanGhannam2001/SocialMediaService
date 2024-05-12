using static System.Diagnostics.Debug;
using MediatR;
using Microsoft.Extensions.Logging;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.RespondToFriendshipRequest;

public sealed class RespondToFriendshipRequestHandler : IRequestHandler<RespondToFriendshipRequestCommand, Result<Unit>>
{
    private readonly ILogger<RespondToFriendshipRequestHandler> _logger;
    private readonly IProfileRepository _repo;

    public RespondToFriendshipRequestHandler(ILogger<RespondToFriendshipRequestHandler> logger,
        IProfileRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<Result<Unit>> Handle(RespondToFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        var friendshipRequest = await _repo.GetFriendshipRequestAsync(request.SenderId, request.ReceiverId, cancellationToken);

        if (friendshipRequest is null)
        {
            return new RecordNotFoundException("You didn't received a friendship request from this user");
        }

        using var transaction = await _repo.BeginTransactionAsync();

        try
        {
            Friendship? friendship = null;

            // Add Friendship if Aggreed
            if (request.Aggreed)
            {
                var sender = await _repo.GetByIdAsync(request.SenderId, cancellationToken);
                var receiver = await _repo.GetByIdAsync(request.ReceiverId, cancellationToken);

                Assert(sender is not null);
                Assert(receiver is not null);

                friendship = new Friendship(sender, receiver);

                await _repo.AddFriendshipAsync(friendship, cancellationToken);
            }

            await _repo.DeleteFriendshipRequestAsync(friendshipRequest, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return Unit.Value;
        }
        catch (Exception exp)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Error happend while trying to handle {0}: {1}", nameof(RespondToFriendshipRequestHandler), exp.Message);
            return new OperationCancelledException("Something went wrong");
        }
    }
}
