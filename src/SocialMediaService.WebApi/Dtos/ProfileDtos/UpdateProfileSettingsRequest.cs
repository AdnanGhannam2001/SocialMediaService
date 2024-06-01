using SocialMediaService.Domain.Enums;

namespace SocialMediaService.WebApi.Dtos.ProfileDtos;

public record UpdateProfileSettingsRequest(InformationVisibilities LastName,
    InformationVisibilities DateOfBirth,
    InformationVisibilities Gender,
    InformationVisibilities Phone,
    InformationVisibilities JobTitle,
    InformationVisibilities Company,
    InformationVisibilities StartDate,
    InformationVisibilities Socials,
    InformationVisibilities Bio);