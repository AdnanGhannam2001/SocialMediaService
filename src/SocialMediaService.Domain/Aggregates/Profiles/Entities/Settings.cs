using SocialMediaService.Domain.Bases;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class Settings : Entity
{
    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public InformationVisibilities LastName { get; private set; }
    public InformationVisibilities DateOfBirth { get; private set; }
    public InformationVisibilities Gender { get; private set; }
    public InformationVisibilities Phone { get; private set; }
    public InformationVisibilities JobTitle { get; private set; }
    public InformationVisibilities Company { get; private set; }
    public InformationVisibilities StartDate { get; private set; }
    public InformationVisibilities Socials { get; private set; }
    public InformationVisibilities Bio { get; private set; }
}