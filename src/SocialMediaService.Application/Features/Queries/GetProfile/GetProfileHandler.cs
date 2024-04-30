using static System.Diagnostics.Debug;
using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Features.Queries.GetProfile;

public sealed class GetProfileHandler : IRequestHandler<GetProfileQuery, Result<GetProfileResult>>
{
    private readonly IProfileRepository _repo;

    public GetProfileHandler(IProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<GetProfileResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        if (request.RequesterId is not null)
        {
            var blocked = await _repo.GetBlockedAsync(request.ProfileId, request.RequesterId, cancellationToken);
            var blockedBy = await _repo.GetBlockedAsync(request.RequesterId, request.ProfileId, cancellationToken);

            if (blocked is not null || blockedBy is not null)
            {
                return new RecordNotFoundException($"Profile is not found");
            }
        }

        var profile = await _repo.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException($"Profile with Id: {request.ProfileId} is not found");
        }

        if (!request.CheckOwnership || request.ProfileId == request.RequesterId)
        {
            return GetProfileResult.MapProfile(profile);
        }

        var settings = await _repo.GetSettingsAsync(request.ProfileId, cancellationToken);

        Assert(settings != null);

        var friendship = request.RequesterId is not null
            ? await _repo.GetFriendshipAsync(request.ProfileId, request.RequesterId, cancellationToken)
            : null;

        var visibilitiy = friendship is null ? InformationVisibilities.Public : InformationVisibilities.Friends;

        return MapToResult(profile, settings, visibilitiy);
    }

    private static GetProfileResult MapToResult(Profile profile, Settings settings, InformationVisibilities visibility)
    {
        return new (profile.Id,
            profile.FirstName,
            settings.LastName       <= visibility ? profile.LastName        : null,
            settings.DateOfBirth    <= visibility ? profile.DateOfBirth     : null,
            settings.Gender         <= visibility ? profile.Gender          : null,
            settings.Phone          <= visibility ? profile.PhoneNumber     : null,
            settings.Bio            <= visibility ? profile.Bio             : null,
            settings.JobTitle       <= visibility ? profile.JobInformations : null,
            settings.Socials        <= visibility ? profile.Socials         : null);
    }
}
