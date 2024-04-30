using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetFriendshipsPage;

public sealed class GetFriendshipsPageHandler : IRequestHandler<GetFriendshipsPageQuery, Result<Page<Friendship>>>
{
    private readonly IProfileRepository _repo;

    public GetFriendshipsPageHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Page<Friendship>>> Handle(GetFriendshipsPageQuery request, CancellationToken cancellationToken)
    {
        if (request.RequesterId is not null &&
            await ProfileHelper.IsBlocked(_repo, request.ProfileId, request.RequesterId, cancellationToken))
        {
            return new RecordNotFoundException($"Profile is not found");
        }

        var page = await _repo.GetFriendshipsPageAsync(request.ProfileId, request.Request, cancellationToken);

        return page;
    }
}
