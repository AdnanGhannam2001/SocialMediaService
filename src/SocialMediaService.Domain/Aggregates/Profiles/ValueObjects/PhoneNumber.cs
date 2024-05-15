using System.Text.RegularExpressions;

namespace SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;

public sealed record PhoneNumber
{
    private static readonly string RegexExpression = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";

    #pragma warning disable CS8618
    private PhoneNumber() { }
    #pragma warning restore CS8618

    public PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; set; }

    public static bool IsValidPhoneNumber(string value)
    {
        var regex = new Regex(RegexExpression);

        return regex.Match(value).Success;
    }
}