using MediatR;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetFavoriteDiscussionsPage;

public sealed class GetFavoriteDiscussionsPageHandler
    : IRequestHandler<GetFavoriteDiscussionsPageQuery, Result<Page<FavoriteDiscussion>>>
{
    private readonly IProfileRepository _profileRepo;

    public GetFavoriteDiscussionsPageHandler(IProfileRepository profileRepo)
    {
        _profileRepo = profileRepo;
    }

    public async Task<Result<Page<FavoriteDiscussion>>> Handle(GetFavoriteDiscussionsPageQuery request, CancellationToken cancellationToken)
    {
        var page = await _profileRepo.GetFavoriteDiscussionsPageAsync(request.ProfileId, request.Request, cancellationToken);

        return page;
    }
}
