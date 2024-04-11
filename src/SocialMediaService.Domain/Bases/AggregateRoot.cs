using NanoidDotNet;

namespace SocialMediaService.Domain.Bases;

public abstract class AggregateRoot<T>
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
}

public abstract class AggregateRoot() : AggregateRoot<string>(Nanoid.Generate(size: 15));