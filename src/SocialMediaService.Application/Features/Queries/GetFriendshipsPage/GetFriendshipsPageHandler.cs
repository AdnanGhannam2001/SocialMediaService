using MediatR;
using PR2.Shared.Common;
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
        var page = await _repo.GetFriendshipsPageAsync(request.UserId, request.Request, cancellationToken);

        return page;
    }
}
