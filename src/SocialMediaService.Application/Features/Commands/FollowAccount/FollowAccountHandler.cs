using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.FollowAccount;

public sealed class FollowAccountHandler : IRequestHandler<FollowAccountCommand, Result<Follow>>
{
    private readonly IProfileRepository _repo;

    public FollowAccountHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Follow>> Handle(FollowAccountCommand request, CancellationToken cancellationToken)
    {
        var follower = await _repo.GetByIdAsync(request.FollowerId, cancellationToken);

        if (follower is null)
        {
            return new RecordNotFoundException("Follower account is not found");
        }

        var profile = await _repo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Account is not found");
        }

        var follow = new Follow(follower, profile);
        await _repo.AddFollowAsync(follow, cancellationToken);

        return follow;
    }
}
