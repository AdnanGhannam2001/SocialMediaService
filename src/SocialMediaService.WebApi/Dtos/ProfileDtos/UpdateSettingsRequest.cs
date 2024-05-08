using SocialMediaService.Domain.Enums;

namespace SocialMediaService.WebApi.Dtos.ProfileDtos;

public record UpdateSettingsRequest(InformationVisibilities LastName,
    InformationVisibilities DateOfBirth,
    InformationVisibilities Gender,
    InformationVisibilities Phone,
    InformationVisibilities JobTitle,
    InformationVisibilities Company,
    InformationVisibilities StartDate,
    InformationVisibilities Socials,
    InformationVisibilities Bio);