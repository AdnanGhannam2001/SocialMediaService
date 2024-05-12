using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.WebApi.Dtos.ProfileDtos;

public record UpdateRequest(string? FirstName = null,
    string? LastName = null,
    DateTime? DateOfBirth = null,
    Genders? Gender = null,
    string? PhoneNumber = null,
    string? Bio = null,
    JobInformations? JobInformations = null,
    string? Facebook = null,
    string? Youtube = null,
    string? Twitter = null);