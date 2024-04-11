using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;
using SocialMediaService.Domain.Bases;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Profiles;

public class Profile : AggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Genders Gender { get; private set; }
    public string Bio { get; private set; }
    public JobInformations JobInformations { get; private set; }
    public Socials Socials { get; private set; }
}