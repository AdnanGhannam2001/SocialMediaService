using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetBlockedPage;

public sealed class GetBlockedPageHandler : IRequestHandler<GetBlockedPageQuery, Result<Page<Block>>>
{
    private readonly IProfileRepository _repo;

    public GetBlockedPageHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Page<Block>>> Handle(GetBlockedPageQuery request, CancellationToken cancellationToken)
    {
        var list = await _repo.GetBlockedPageAsync(request.ProfileId, request.Request, cancellationToken);

        return list;
    }
}
