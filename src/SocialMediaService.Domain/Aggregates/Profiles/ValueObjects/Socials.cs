using SocialMediaService.Domain.Exceptions;

namespace SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;

public sealed record Socials
{
    public Socials() { }

    public Socials(string? facebook = null, string? youtube = null, string? twitter = null)
    {
        if (!IsValidUrlOrEmpty(facebook)) throw new InvalidUrlException(nameof(Facebook));
        if (!IsValidUrlOrEmpty(youtube)) throw new InvalidUrlException(nameof(Youtube));
        if (!IsValidUrlOrEmpty(twitter)) throw new InvalidUrlException(nameof(Twitter));

        Facebook = facebook;
        Youtube = youtube;
        Twitter = twitter;
    }

    public string? Facebook { get; }
    public string? Youtube { get; }
    public string? Twitter { get; }

    private static bool IsValidUrlOrEmpty(string? value)
        => value is null
            || (Uri.TryCreate(value, UriKind.Absolute, out var result)
                && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps));
}