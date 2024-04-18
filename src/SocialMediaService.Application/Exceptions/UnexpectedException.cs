using PR2.Shared.Common;

namespace SocialMediaService.Application.Exceptions;

public class UnexpectedException : ExceptionBase
{
    public UnexpectedException()
        : base("None", "Unexpected Exception occurred") { }
}
