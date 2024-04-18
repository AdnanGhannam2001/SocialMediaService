using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Queries.GetProfile;

public sealed record GetProfileResult(string Id,
    string FirstName,
    string? LastName = null,
    DateTime? DateOfBirth = null,
    Genders? Gender = null,
    PhoneNumber? PhoneNumber = null,
    string? Bio = null,
    JobInformations? JobInformations = null,
    Socials? Socials = null)
{
    public static GetProfileResult MapProfile(Profile profile)
    {
        return new (profile.Id,
            profile.FirstName,
            profile.LastName,
            profile.DateOfBirth,
            profile.Gender,
            profile.PhoneNumber,
            profile.Bio,
            profile.JobInformations,
            profile.Socials);
    }
}