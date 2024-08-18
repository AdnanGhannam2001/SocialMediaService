using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.WebApi.Configurations;

namespace SocialMediaService.WebApi.Services;

using FileName = string;

public sealed class FilesService
{
    public async Task<Result<FileName>> SaveImageAsync(IFormFile file, Storage.FileOptions options, string name)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!options.AllowedExtension.Contains(extension))
        {
            return new(new DataValidationException("Image", "Invalid file extension"));
        }

        if (options.MaxAllowedSize < file.Length)
        {
            return new(new DataValidationException("Image", "File is too big"));
        }

        var fullPath = Path.Combine(options.Path, name + extension);
        Directory.CreateDirectory(options.Path);
        using var stream = File.Create(fullPath);
        await file.CopyToAsync(stream);

        return fullPath;
    }

    public Result<FileName> DeleteImage(string path, string name)
    {
        var file = Directory.GetFiles(path, name + ".*").FirstOrDefault();

        if (file is null)
        {
            return new(new DataValidationException("Image", "Image not found"));
        }

        File.Delete(file);

        return file;
    }
}