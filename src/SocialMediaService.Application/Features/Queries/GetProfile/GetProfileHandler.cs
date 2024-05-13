using static System.Diagnostics.Debug;
using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;
using SocialMediaService.Application.Helpers;

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
        if (request.RequesterId is not null &&
            await ProfileHelper.IsBlocked(_repo, request.ProfileId, request.RequesterId, cancellationToken))
        {
            return new RecordNotFoundException($"Profile is not found");
        }

        var profile = await _repo.GetWithSettingsAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException($"Profile with Id: {request.ProfileId} is not found");
        }

        if (!request.CheckOwnership || request.ProfileId == request.RequesterId)
        {
            return GetProfileResult.MapProfile(profile);
        }

        // TODO: Test this
        var requester = request.RequesterId is not null
            ? await _repo.GetWithFriendshipAsync(request.ProfileId, request.RequesterId, cancellationToken)
                ?? await _repo.GetWithFriendshipAsync(request.RequesterId, request.ProfileId, cancellationToken)
            : null;

        var visibilitiy = (requester is null || requester.Friends.Count == 0)
            ? InformationVisibilities.Public
            : InformationVisibilities.Friends;

        return MapToResult(profile, visibilitiy);
    }

    // TODO: Maybe Add to ProfileHelper
    private static GetProfileResult MapToResult(Profile profile, InformationVisibilities visibility)
    {
        return new (profile.Id,
            profile.FirstName,
            profile.Settings.LastName       <= visibility ? profile.LastName        : null,
            profile.Settings.DateOfBirth    <= visibility ? profile.DateOfBirth     : null,
            profile.Settings.Gender         <= visibility ? profile.Gender          : null,
            profile.Settings.Phone          <= visibility ? profile.PhoneNumber     : null,
            profile.Settings.Bio            <= visibility ? profile.Bio             : null,
            profile.Settings.JobTitle       <= visibility ? profile.JobInformations : null,
            profile.Settings.Socials        <= visibility ? profile.Socials         : null);
    }
}
