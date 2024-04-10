using NanoidDotNet;

namespace SocialMediaService.Domain.Bases;

public abstract class Entity<T>
    where T : IComparable<T>
{
    public Entity(T id)
    {
        Id = id;
    }

    public T Id { get; init; }
}

public abstract class Entity() : Entity<string>(Nanoid.Generate(size: 15));