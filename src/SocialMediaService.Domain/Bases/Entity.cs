using NanoidDotNet;

namespace SocialMediaService.Domain.Bases;

public abstract class Entity<T>
    where T : IComparable<T>
{
    public Entity(T id)
    {
        Id = id;
        CreatedAtUtc = UpdatedAtUtc = DateTime.UtcNow;
    }

    public T Id { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; protected set; }
}

public abstract class Entity() : Entity<string>(Nanoid.Generate(size: 15));