using SocialMediaService.Domain.Bases;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class Settings : Entity
{
    public Settings(Profile profile) : base()
    {
        Id = profile.Id;
        Profile = profile;
    }

    public Profile Profile { get; private set; }

    public InformationVisibilities LastName { get; private set; } = InformationVisibilities.Public;
    public InformationVisibilities DateOfBirth { get; private set; } = InformationVisibilities.Public;
    public InformationVisibilities Gender { get; private set; } = InformationVisibilities.Public;
    public InformationVisibilities Phone { get; private set; } = InformationVisibilities.Public;
    public InformationVisibilities JobTitle { get; private set; } = InformationVisibilities.Public;
    public InformationVisibilities Company { get; private set; } = InformationVisibilities.Public;
    public InformationVisibilities StartDate { get; private set; } = InformationVisibilities.Public;
    public InformationVisibilities Socials { get; private set; } = InformationVisibilities.Public;
    public InformationVisibilities Bio { get; private set; } = InformationVisibilities.Public;
}