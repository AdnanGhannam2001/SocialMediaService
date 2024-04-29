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
        var friendship = await _repo.GetFriendshipAsync(request.Id1, request.Id2, cancellationToken);

        if (friendship is null)
        {
            return new RecordNotFoundException("No Friendship found");
        }

        await _repo.DeleteFriendshipAsync(friendship, cancellationToken);        

        return friendship;
    }
}
