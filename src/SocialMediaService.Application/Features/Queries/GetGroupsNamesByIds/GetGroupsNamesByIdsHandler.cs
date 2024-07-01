using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetGroupsNamesByIds;

public sealed class GetGroupsNamesByIdsHandler : IRequestHandler<GetGroupsNamesByIdsQuery, Result<IEnumerable<GetGroupsNamesByIdsResult>>>
{
    private readonly IGroupRepository _repo;

    public GetGroupsNamesByIdsHandler(IGroupRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<IEnumerable<GetGroupsNamesByIdsResult>>> Handle(GetGroupsNamesByIdsQuery request, CancellationToken cancellationToken)
    {
        var page = await _repo.GetGroupsByIdsAsync<GetGroupsNamesByIdsResult>(request.Ids,
                x => new (x.Id, x.Name),
                cancellationToken);

        return new (page);
    }
}
