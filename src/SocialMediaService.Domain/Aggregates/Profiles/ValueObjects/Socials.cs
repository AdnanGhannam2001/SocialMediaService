namespace SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;

public sealed record Socials
{
    public Socials(string? facebook = null, string? youtube = null, string? twitter = null)
    {
        Facebook = facebook;
        Youtube = youtube;
        Twitter = twitter;
    }

    public string? Facebook { get; }
    public string? Youtube { get; }
    public string? Twitter { get; }
}