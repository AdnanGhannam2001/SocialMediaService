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

        blocker.RemoveBlocked(blocked);
        await _repo.SaveChangesAsync(cancellationToken);

        return blocked;
    }
}
