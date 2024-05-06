using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.UnfollowAccount;

public sealed class UnfollowAccountHandler : IRequestHandler<UnfollowAccountCommand, Result<Follow>>
{
    private readonly IProfileRepository _repo;

    public UnfollowAccountHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Follow>> Handle(UnfollowAccountCommand request, CancellationToken cancellationToken)
    {
        var follower = await _repo.GetByIdAsync(request.FollowerId, cancellationToken);

        if (follower is null)
        {
            return new RecordNotFoundException("Follower account is not found");
        }

        var profile = await _repo.GetByIdAsync(request.FollowedId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Account is not found");
        }

        var follow = await _repo.GetFollowedAsync(request.FollowerId, request.FollowedId, cancellationToken);

        if (follow is null)
        {
            return new RecordNotFoundException("You're not following this account");
        }

        await _repo.DeleteFollowAsync(follow, cancellationToken);

        return follow;
    }
}
