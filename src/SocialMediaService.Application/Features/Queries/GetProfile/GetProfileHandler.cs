using MediatR;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;
using SocialMediaService.Application.Helpers;
using SocialMediaService.Persistent.Data;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaService.Application.Features.Queries.GetProfile;

public sealed class GetProfileHandler : IRequestHandler<GetProfileQuery, Result<GetProfileResult>>
{
    private readonly ApplicationDbContext _context;
    private readonly IProfileRepository _repo;

    public GetProfileHandler(ApplicationDbContext context, IProfileRepository repo)
    {
        _context = context;
        _repo = repo;
    }

    public async Task<Result<GetProfileResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        if (request.RequesterId is not null &&
            await ProfileHelper.IsBlocked(_repo, request.ProfileId, request.RequesterId, cancellationToken))
        {
            return new RecordNotFoundException($"Profile is not found");
        }

        var profile = await _context.Profiles
            .Include(x => x.Settings)
            .Include(x => x.Friends.Where(x => x.FriendId.Equals(request.RequesterId) || x.ProfileId.Equals(request.RequesterId)))
            .Include(x => x.FollowedBy.Where(x => x.FollowerId.Equals(request.RequesterId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(request.ProfileId), cancellationToken);

        if (profile is null)
        {
            return new RecordNotFoundException($"Profile with Id: {request.ProfileId} is not found");
        }

        var followers = await _repo.CountFollowedAsync(profile.Id, null, cancellationToken);
        var following = await _repo.CountFollowingAsync(profile.Id, null, cancellationToken);

        if (!request.CheckOwnership || request.ProfileId == request.RequesterId)
        {
            return GetProfileResult.MapProfile(profile, followers, following);
        }

        // TODO: Test this
        var requester = request.RequesterId is not null
            ? await _repo.GetWithFriendshipAsync(request.ProfileId, request.RequesterId, cancellationToken)
                ?? await _repo.GetWithFriendshipAsync(request.RequesterId, request.ProfileId, cancellationToken)
            : null;

        var visibilitiy = (requester is null || requester.Friends.Count == 0)
            ? InformationVisibilities.Public
            : InformationVisibilities.Friends;

        return MapToResult(profile, visibilitiy, followers, following);
    }

    private static GetProfileResult MapToResult(Profile profile, InformationVisibilities visibility, int followers, int following)
    {
        return new (profile.Id,
            profile.FirstName,
            profile.CreatedAtUtc,
            profile.UpdatedAtUtc,
            profile.Image,
            profile.CoverImage,
            profile.Settings.LastName       <= visibility ? profile.LastName        : null,
            profile.Settings.DateOfBirth    <= visibility ? profile.DateOfBirth     : null,
            profile.Settings.Gender         <= visibility ? profile.Gender          : null,
            profile.Settings.Phone          <= visibility ? profile.PhoneNumber     : null,
            profile.Settings.Bio            <= visibility ? profile.Bio             : null,
            profile.Settings.JobTitle       <= visibility ? profile.JobInformations : null,
            profile.Settings.Socials        <= visibility ? profile.Socials         : null,
            profile.Settings,
            followers,
            following);
    }
}
