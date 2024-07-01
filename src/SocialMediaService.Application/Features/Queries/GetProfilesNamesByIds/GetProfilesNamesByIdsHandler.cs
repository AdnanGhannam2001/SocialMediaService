using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetProfilesNamesByIds;

public sealed class GetProfilesNamesByIdsHandler : IRequestHandler<GetProfilesNamesByIdsQuery, Result<IEnumerable<GetProfilesNamesByIdsResult>>>
{
    private readonly IProfileRepository _repo;

    public GetProfilesNamesByIdsHandler(IProfileRepository profileRepo)
    {
        _repo = profileRepo;
    }

    public async Task<Result<IEnumerable<GetProfilesNamesByIdsResult>>> Handle(GetProfilesNamesByIdsQuery request, CancellationToken cancellationToken)
    {
        var page = await _repo.GetProfilesByIdsAsync<GetProfilesNamesByIdsResult>(request.Ids,
                x => new (x.Id, x.FirstName, x.LastName),
                cancellationToken);

        return new (page);
    }
}