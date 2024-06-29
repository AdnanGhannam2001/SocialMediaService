using Microsoft.AspNetCore.Mvc;

namespace SocialMediaService.WebApi.Controllers;

[Route("[controller]")]
public sealed class AuthController : ControllerBase
{
    [HttpGet("login")]
    public IActionResult RedirectTodentityProvider()
    {
        return Challenge("oidc");
    }
}