using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Commands.CreateProfile;

public sealed record CreateProfileCommand(string Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    Genders Gender,
    PhoneNumber? PhoneNumber = null) : ICommand<Profile>;