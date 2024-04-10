using NanoidDotNet;

namespace SocialMediaService.Domain.Bases;

public abstract class AggregateRoot<T>
    where T : IComparable<T>
{
    public AggregateRoot(T id)
    {
        Id = id;
    }

    public T Id { get; init; }
}

public abstract class AggregateRoot() : AggregateRoot<string>(Nanoid.Generate(size: 15));