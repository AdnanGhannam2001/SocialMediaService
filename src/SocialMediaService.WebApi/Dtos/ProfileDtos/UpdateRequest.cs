using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.WebApi.Dtos.ProfileDtos;

public record UpdateRequest(string? FirstName = null,
    string? LastName = null,
    DateTime? DateOfBirth = null,
    Genders? Gender = null,
    PhoneNumber? PhoneNumber = null,
    string? Bio = null,
    JobInformations? JobInformations = null,
    Socials? Socials = null);