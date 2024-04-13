using SocialMediaService.Domain.Bases;

namespace SocialMediaService.Domain.Aggregates.Groups;

public sealed class Tag : Entity
{
    private List<Discussion> _discussions = [];

    #pragma warning disable CS8618
    private Tag() { }
    #pragma warning restore CS8618

    public Tag(string label)
    {
        Label = label;
    }

    public string Label { get; private set; }

    public IReadOnlyCollection<Discussion> Discussions => _discussions;
}