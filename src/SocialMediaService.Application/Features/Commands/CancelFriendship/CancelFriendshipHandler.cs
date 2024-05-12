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
        var friendship1 = await _repo.GetFriendshipAsync(request.Id1, request.Id2, cancellationToken);
        var friendship2 = await _repo.GetFriendshipAsync(request.Id2, request.Id1, cancellationToken);

        if (friendship1 is null && friendship2 is null)
        {
            return new RecordNotFoundException("No Friendship found");
        }

        await _repo.DeleteFriendshipAsync((friendship1 ?? friendship2)!, cancellationToken);        

        return (friendship1 ?? friendship2)!;
    }
}
