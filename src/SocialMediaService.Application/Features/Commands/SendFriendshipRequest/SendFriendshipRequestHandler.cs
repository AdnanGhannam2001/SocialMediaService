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
        if (request.SenderId == request.ReceiverId)
        {
            return new DataValidationException(nameof(request.SenderId), "Sender and receiver can't be the same");
        }

        if (await ProfileHelper.IsBlocked(_repo, request.SenderId, request.ReceiverId, cancellationToken))
        {
            return new RecordNotFoundException($"Profile is not found");
        }

        var friendship = await _repo.GetFriendshipAsync(request.SenderId, request.ReceiverId, cancellationToken);

        if (friendship is not null)
        {
            return new DuplicatedRecordException("You're already a friend with this user");
        }

        var sender = await _repo.GetByIdAsync(request.SenderId, cancellationToken);
        var receiver = await _repo.GetByIdAsync(request.ReceiverId, cancellationToken);

        ArgumentNullException.ThrowIfNull(sender);
        ArgumentNullException.ThrowIfNull(receiver);

        var friendshipRequest = new FriendshipRequest(sender, receiver);

        await _repo.AddFriendshipRequestAsync(friendshipRequest, cancellationToken);

        return friendshipRequest;
    }
}
