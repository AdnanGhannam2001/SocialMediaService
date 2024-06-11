namespace SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;

public sealed record Socials
{
    private Socials() { }

    public Socials(string? facebook = null, string? youtube = null, string? twitter = null)
    {
        Facebook = facebook;
        Youtube = youtube;
        Twitter = twitter;
    }

    public string? Facebook { get; }
    public string? Youtube { get; }
    public string? Twitter { get; }

    public static bool AreValidLinks(Socials socials)
    {
        return IsValidUrlOrEmpty(socials.Facebook) && IsValidUrlOrEmpty(socials.Youtube) && IsValidUrlOrEmpty(socials.Twitter);
    }

    private static bool IsValidUrlOrEmpty(string? value)
        => string.IsNullOrEmpty(value)
            || (Uri.TryCreate(value, UriKind.Absolute, out var result)
                && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps));
}