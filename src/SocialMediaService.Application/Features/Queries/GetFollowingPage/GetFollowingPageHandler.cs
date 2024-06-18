using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetFollowingPage;

public sealed class GetFollowsPageHandler : IRequestHandler<GetFollowingPageQuery, Result<Page<Follow>>>
{
    private readonly IProfileRepository _repo;

    public GetFollowsPageHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Page<Follow>>> Handle(GetFollowingPageQuery request, CancellationToken cancellationToken)
    {
        var follow = await _repo.GetFollowingPageAsync(request.UserId, request.Request, cancellationToken);

        return follow;
    }
}
