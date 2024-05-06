using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetSettings;

public sealed class GetSettingsHandler : IRequestHandler<GetSettingsQuery, Result<Settings>>
{
    private readonly IProfileRepository _repo;

    public GetSettingsHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Settings>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await _repo.GetSettingsAsync(request.Id, cancellationToken);

        if (settings is null)
        {
            return new RecordNotFoundException("Profile not found");
        }

        return settings;
    }
}
