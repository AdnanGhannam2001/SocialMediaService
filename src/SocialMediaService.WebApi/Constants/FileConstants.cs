using System.Collections.ObjectModel;

namespace SocialMediaService.WebApi.Constants;

public static class FileConstants
{
    public static readonly ReadOnlyDictionary<string, string> ExtensionToMime =
        new Dictionary<string, string>()
            {
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".bmp", "image/bmp" },
                { ".txt", "text/plain" },
            }
        .AsReadOnly();
}