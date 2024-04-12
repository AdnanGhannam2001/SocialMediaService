using PR2.Shared.Common;

namespace SocialMediaService.Domain.Exceptions;

public class InvalidUrlException : ExceptionBase
{
    public InvalidUrlException(string propertyName) : base(propertyName, "Invalid Url") { }
}