using System.Text.RegularExpressions;
using SocialMediaService.Domain.Exceptions;

namespace SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;

public sealed record PhoneNumber
{
    private readonly string _regexExpression = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";

    public PhoneNumber(string value)
    {
        var regex = new Regex(_regexExpression);

        if (!regex.Match(value).Success) throw new InvalidPhoneNumberException();

        Value = value;
    }

    public string Value { get; set; }
}