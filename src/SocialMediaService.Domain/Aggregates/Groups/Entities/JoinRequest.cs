using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Domain.Aggregates.Groups;

public class JoinRequest
{
    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string GroupId { get; private set; }
    public Group Group { get; private set; }

    public string? Content { get; private set; }
    public DateTime SentAt { get; private set; }
}