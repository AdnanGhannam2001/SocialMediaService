namespace SocialMediaService.WebApi.Configurations;

public sealed class Storage
{
    public class FileOptions
    {
        public string Path { get; set; } = string.Empty;
        public string[] AllowedExtension { get; set; } = [];
        public float MaxAllowedSize { get; set; } = 0.0f;
    }

    public Dictionary<string, FileOptions> FilesOptions { get; set; } = [];
}