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
        var follower = await _repo.GetWithFollowedAsync(request.FollowerId, request.FollowedId, cancellationToken);

        if (follower is null)
        {
            return new RecordNotFoundException("Follower profile is not found");
        }

        if (follower.Following.Count == 0)
        {
            return new RecordNotFoundException("You're not following this user");
        }

        var follow = follower.Following.ElementAt(0);
        follower.RemoveFollow(follow);
        await _repo.SaveChangesAsync(cancellationToken);

        return follow;
    }
}
