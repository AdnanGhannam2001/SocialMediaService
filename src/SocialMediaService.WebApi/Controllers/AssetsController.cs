using Microsoft.AspNetCore.Mvc;
using static SocialMediaService.WebApi.Constants.FileConstants;

namespace SocialMediaService.WebApi.Controllers;

[Route("[controller]")]
public sealed class AssetsController : ControllerBase
{
    private readonly IHostEnvironment _environment;

    public AssetsController(IHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpGet("{*filename}")]
    public IActionResult GetFile(string filename)
    {
        var path = Path.Combine(_environment.ContentRootPath, "Assets");

        var file = Directory.GetFiles(path, $"{filename}.*").FirstOrDefault();

        if (file is null)
        {
            return NotFound();
        }

        var extension = Path.GetExtension(file);
        return PhysicalFile(Path.Combine(path, file), ExtensionToMime[extension]);
    }
}