using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Commands.CancelFriendship;

public sealed class CancelFriendshipHandler : IRequestHandler<CancelFriendshipCommand, Result<Friendship>>
{
    private readonly IProfileRepository _repo;

    public CancelFriendshipHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Friendship>> Handle(CancelFriendshipCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repo.GetWithFriendshipAsync(request.ProfileId, request.FriendId, cancellationToken);
        var friend = await _repo.GetWithFriendshipAsync(request.FriendId, request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException("Profile is not found");
        }

        if (friend is null)
        {
            return new RecordNotFoundException("Friend profile is not found");
        }

        if (profile.Friends.Count == 0 && friend.Friends.Count == 0)
        {
            return new RecordNotFoundException("No Friendship found");
        }

        if (profile.Friends.ElementAt(0) is var friendship1 && friendship1 is not null)
        {
            profile.RemoveFriend(friendship1);
        }

        if (friend.Friends.ElementAt(0) is var friendship2 && friendship2 is not null)
        {
            friend.RemoveFriend(friendship2);
        }

        await _repo.SaveChangesAsync(cancellationToken);        

        return (friendship1 ?? friendship2)!;
    }
}
