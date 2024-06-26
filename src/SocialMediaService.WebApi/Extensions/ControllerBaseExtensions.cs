using Microsoft.AspNetCore.Mvc;
using PR2.Shared.Common;
using PR2.Shared.Exceptions;

namespace SocialMediaService.WebApi.Extensions;

internal static class ControllerBaseExtensions
{
    public static IActionResult GetFromResult<T>(this ControllerBase controller, Result<T> result, int successCode = 200)
        where T : notnull
    {
        if (result.IsSuccess)
        {
            return successCode switch
            {
                204 => controller.NoContent(),
                _ => controller.StatusCode(successCode, result.Value)
            };
        }

        if (result.Exceptions.Length > 1)
        {
            return controller.BadRequest(result.Exceptions);
        }

        return result.Exceptions[0] switch
        {
            RecordNotFoundException e => controller.NotFound(e),
            UnauthorizedException e => controller.Unauthorized(e),
            _ => controller.BadRequest(result.Exceptions[0])
        };
    }
}