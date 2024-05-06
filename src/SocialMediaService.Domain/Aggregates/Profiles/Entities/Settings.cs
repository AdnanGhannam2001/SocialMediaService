using SocialMediaService.Domain.Bases;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class Settings : Entity
{
    #pragma warning disable CS8618
    private Settings() { }

    public Settings(string profileId) : base()
    {
        Id = profileId;
    }
    #pragma warning restore CS8618

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

    public void Update(Settings settings)
    {
        LastName = settings.LastName;
        DateOfBirth = settings.DateOfBirth;
        Gender = settings.Gender;
        Phone = settings.Phone;
        JobTitle = settings.JobTitle;
        Company = settings.Company;
        StartDate = settings.StartDate;
        Socials = settings.Socials;
        Bio = settings.Bio;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}