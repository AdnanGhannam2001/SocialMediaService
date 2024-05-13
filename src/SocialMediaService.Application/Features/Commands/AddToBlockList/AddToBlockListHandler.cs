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
        if (request.BlockerId == request.ProfileId)
        {
            return new DataValidationException(nameof(request.BlockerId), "Blocker and profile to be blocked can't be the same");
        }

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

        var block = new Block(blocker, profile, request.Reason);
        blocker.AddBlocked(block);
        await _repo.SaveChangesAsync(cancellationToken);

        return block;
    }
}
