using PR2.Shared.Common;
using PR2.Shared.Exceptions;
using SocialMediaService.Domain.Enums;
using SocialMediaService.WebApi.Configurations;

namespace SocialMediaService.WebApi.Services;

using FileName = string;

public sealed class FilesService
{
    public Result<bool> Validate(IFormFile file, Storage.FileOptions options)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!options.AllowedExtension.Contains(extension))
        {
            return new Result<bool>(
                    new DataValidationException("File", "Invalid file extension"));
        }

        if (options.MaxAllowedSize < file.Length)
        {
            return new Result<bool>(
                    new DataValidationException("File", "File is too big"));
        }

        return true;
    }

    public Task<FileName> SaveImageAsync(IFormFile file, Storage.FileOptions options, string name)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();
        return SaveFileAsync(file, options, name + extension);
    }

    public async Task<FileName> SaveFileAsync(IFormFile file, Storage.FileOptions options, string name)
    {
        var fullPath = Path.Combine(options.Path, name);
        Directory.CreateDirectory(options.Path);
        using var stream = File.Create(fullPath);
        await file.CopyToAsync(stream);

        return fullPath;
    }

    public Result<FileName> DeleteFile(string path, string name)
    {
        var file = Directory.GetFiles(path, name + ".*").FirstOrDefault();

        if (file is null)
        {
            return new(new DataValidationException("File", "File not found"));
        }

        File.Delete(file);

        return file;
    }

    public MediaTypes? GetFileMediaType(IFormFile? file)
    {
        if (file is null)
        {
            return null;
        }

        var extension = Path.GetExtension(file.FileName);

        return extension switch
        {
            ".png" or ".jpeg" or ".jpg" or ".bmp" => MediaTypes.Image,
            ".mp4" or ".avi" => MediaTypes.Video,
            _ => MediaTypes.File
        };
    }
}