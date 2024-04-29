using MediatR;
using Microsoft.Extensions.Logging;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.RespondToFriendshipRequest;

public sealed class RespondToFriendshipRequestHandler : IRequestHandler<RespondToFriendshipRequestCommand, Result<Friendship>>
{
    private readonly ILogger<RespondToFriendshipRequestHandler> _logger;
    private readonly IProfileRepository _repo;

    public RespondToFriendshipRequestHandler(ILogger<RespondToFriendshipRequestHandler> logger,
        IProfileRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<Result<Friendship>> Handle(RespondToFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        var friendshipRequest = await _repo.GetFriendshipRequestAsync(request.SenderId, request.ReceiverId, cancellationToken);

        if (friendshipRequest is null)
        {
            return new RecordNotFoundException("You didn't send a friendship request to this user");
        }

        using var transaction = await _repo.BeginTransactionAsync();

        try
        {
            Friendship friendship;

            // Add Friendship
            {
                var sender = await _repo.GetByIdAsync(request.SenderId, cancellationToken);
                var receiver = await _repo.GetByIdAsync(request.ReceiverId, cancellationToken);

                ArgumentNullException.ThrowIfNull(sender);
                ArgumentNullException.ThrowIfNull(receiver);

                friendship = new Friendship(sender, receiver);

                await _repo.CreateFriendshipAsync(friendship, cancellationToken);
            }

            await _repo.DeleteFriendshipRequestAsync(friendshipRequest, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return friendship;
        }
        catch (Exception exp)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Error happend while trying to handle {0}: {1}", nameof(RespondToFriendshipRequestHandler), exp.Message);
            return new OperationCancelledException("Something went wrong");
        }
    }
}
