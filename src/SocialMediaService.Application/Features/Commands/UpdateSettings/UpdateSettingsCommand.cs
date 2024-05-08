using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Commands.UpdateSettings;

public sealed record UpdateSettingsCommand(string Id,
    InformationVisibilities LastName,
    InformationVisibilities DateOfBirth,
    InformationVisibilities Gender,
    InformationVisibilities Phone,
    InformationVisibilities JobTitle,
    InformationVisibilities Company,
    InformationVisibilities StartDate,
    InformationVisibilities Socials,
    InformationVisibilities Bio) : ICommand<Settings>;