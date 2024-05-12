using System.Text.RegularExpressions;
using SocialMediaService.Domain.Exceptions;

namespace SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;

public sealed record PhoneNumber
{
    private readonly string _regexExpression = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";

    #pragma warning disable CS8618
    private PhoneNumber() { }
    #pragma warning restore CS8618

    /// <summary>
    /// Initialize a PhoneNumber by setting PhoneNumber.Value
    /// </summary>
    /// <param name="value">The actual value</param>
    /// <exception cref="InvalidPhoneNumberException">PhoneNumber.Value is not a valid phonenumber</exception>
    public PhoneNumber(string value)
    {
        var regex = new Regex(_regexExpression);

        if (!regex.Match(value).Success) throw new InvalidPhoneNumberException();

        Value = value;
    }

    public string Value { get; set; }
}