using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Queries.GetProfile;

public sealed record GetProfileResult(string Id,
    string FirstName,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    bool? Image = null,
    bool? CoverImage = null,
    string? LastName = null,
    DateTime? DateOfBirth = null,
    Genders? Gender = null,
    PhoneNumber? PhoneNumber = null,
    string? Bio = null,
    JobInformations? JobInformations = null,
    Socials? Socials = null,
    Settings? Settings = null,
    int Followers = 0,
    int Following = 0)
{
    public static GetProfileResult MapProfile(Profile profile, int followers, int following)
    {
        return new (profile.Id,
            profile.FirstName,
            profile.CreatedAtUtc,
            profile.UpdatedAtUtc,
            profile.Image,
            profile.CoverImage,
            profile.LastName,
            profile.DateOfBirth,
            profile.Gender,
            profile.PhoneNumber,
            profile.Bio,
            profile.JobInformations,
            profile.Socials,
            profile.Settings,
            followers,
            following);
    }
}