using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Bases;

namespace SocialMediaService.Domain.Aggregates.Groups;

public class Kicked
{
    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string KickedById { get; private set; }
    public Profile KickedBy { get; private set; }

    public string GroupId { get; private set; }
    public Group Group { get; private set; }

    public string Reason { get; private set; }
    public DateTime KickedAt { get; private set; }
}