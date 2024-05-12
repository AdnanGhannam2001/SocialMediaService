using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Commands.UpdateProfile;

public sealed record UpdateProfileCommand(string Id,
    string? FirstName = null,
    string? LastName = null,
    DateTime? DateOfBirth = null,
    Genders? Gender = null,
    string? PhoneNumber = null,
    string? Bio = null,
    JobInformations? JobInformations = null,
    string? Facebook = null,
    string? Youtube = null,
    string? Twitter = null) : ICommand<Profile>;