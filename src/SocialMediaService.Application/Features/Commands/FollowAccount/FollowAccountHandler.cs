using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
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
        if (await ProfileHelper.IsBlocked(_repo, request.FollowerId, request.ProfileId, cancellationToken))
        {
            return new RecordNotFoundException("Profile is blocked or not found");
        }

        var follower = await _repo.GetWithFollowedAsync(request.FollowerId, request.ProfileId, cancellationToken);

        if (follower is null)
        {
            return new RecordNotFoundException("Follower profile is not found");
        }

        if (follower.Following.Count > 0)
        {
            return new DuplicatedRecordException("You're already following this user");
        }

        var profile = await _repo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        var follow = new Follow(follower, profile);
        profile.AddFollow(follow);
        await _repo.SaveChangesAsync(cancellationToken);

        return follow;
    }
}
