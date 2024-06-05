using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetProfilesPage;

public sealed class GetProfilesPageHandler : IRequestHandler<GetProfilesPageQuery, Result<Page<Profile>>>
{
    private readonly IProfileRepository _profileRepo;

    public GetProfilesPageHandler(IProfileRepository profileRepo)
    {
        _profileRepo = profileRepo;
    }

    public async Task<Result<Page<Profile>>> Handle(GetProfilesPageQuery request, CancellationToken cancellationToken)
    {
        var page = await _profileRepo.GetPageAsync(request.Request, cancellationToken);

        return page;
    }
}
