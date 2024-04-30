using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
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
        var blocked = await _repo.GetBlockedAsync(request.BlockerId, request.BlockedId, cancellationToken);

        if (blocked is null)
        {
            return new RecordNotFoundException("This profile is not blocked");
        }

        await _repo.DeleteBlockAsync(blocked, cancellationToken);

        return blocked;
    }
}
