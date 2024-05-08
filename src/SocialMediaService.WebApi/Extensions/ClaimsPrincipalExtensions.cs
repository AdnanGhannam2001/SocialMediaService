using System.Security.Claims;

namespace SocialMediaService.WebApi.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static string? GetId(this ClaimsPrincipal principal)
        => principal.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
}