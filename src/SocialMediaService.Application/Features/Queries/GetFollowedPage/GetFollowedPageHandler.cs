using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetFollowedPage;

public sealed class GetFollowsPageHandler : IRequestHandler<GetFollowedPageQuery, Result<Page<Follow>>>
{
    private readonly IProfileRepository _repo;

    public GetFollowsPageHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Page<Follow>>> Handle(GetFollowedPageQuery request, CancellationToken cancellationToken)
    {
        var follow = await _repo.GetFollowedPageAsync(request.UserId, request.Request, cancellationToken);

        return follow;
    }
}
