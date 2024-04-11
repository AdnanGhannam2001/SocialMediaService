using SocialMediaService.Domain.Bases;

namespace SocialMediaService.Domain.Aggregates.Groups;

public sealed class Tag : Entity
{
    private List<Discussion> _discussions = [];

    public Tag(string label)
    {
        Label = label;
    }

    public string Label { get; private set; }

    public IReadOnlyCollection<Discussion> Discussions => _discussions;
}