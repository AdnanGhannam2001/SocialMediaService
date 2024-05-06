using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetFollowsPage;

public sealed class GetFollowsPageHandler : IRequestHandler<GetFollowsPageQuery, Result<Page<Follow>>>
{
    private readonly IProfileRepository _repo;

    public GetFollowsPageHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Page<Follow>>> Handle(GetFollowsPageQuery request, CancellationToken cancellationToken)
    {
        var follow = await _repo.GetFollowedPageAsync(request.UserId, request.Request, cancellationToken);

        return follow;
    }
}
