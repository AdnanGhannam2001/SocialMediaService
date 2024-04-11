using NanoidDotNet;

namespace SocialMediaService.Domain.Bases;

public abstract class AggregateRoot<T> : IEquatable<AggregateRoot<T>>
    where T : IComparable<T>
{
    public AggregateRoot(T id)
    {
        Id = id;
        CreatedAtUtc = UpdatedAtUtc = DateTime.UtcNow;
    }

    public T Id { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; protected set; }

    public bool Equals(AggregateRoot<T>? other) => other is not null && Id.Equals(other.Id);
    public override bool Equals(object? obj) => Equals(obj as AggregateRoot<T>);
    public override int GetHashCode() => GetHashCode() ^ 11;
}

public abstract class AggregateRoot() : AggregateRoot<string>(Nanoid.Generate(size: 15));