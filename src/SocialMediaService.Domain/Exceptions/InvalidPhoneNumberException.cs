using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;

namespace SocialMediaService.Domain.Exceptions;

public class InvalidPhoneNumberException : ExceptionBase
{
    public InvalidPhoneNumberException() : base(nameof(PhoneNumber), "Invalid phone number") { }
}