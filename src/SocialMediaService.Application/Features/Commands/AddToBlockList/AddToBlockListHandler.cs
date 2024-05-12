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

        if (await ProfileHelper.IsBlocked(_repo, request.BlockerId, request.ProfileId, cancellationToken))
        {
            return new RecordNotFoundException("Profile is already blocked or not found");
        }

        var blocker = await _repo.GetByIdAsync(request.BlockerId, cancellationToken);
        var profile = await _repo.GetByIdAsync(request.ProfileId, cancellationToken);

        ArgumentNullException.ThrowIfNull(blocker);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var block = new Block(blocker, profile, request.Reason);

        await _repo.AddBlockAsync(block, cancellationToken);

        return block;
    }
}
