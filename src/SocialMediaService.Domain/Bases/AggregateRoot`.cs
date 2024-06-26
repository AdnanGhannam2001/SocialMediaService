using System.Text;

namespace SocialMediaService.Domain.Bases;

public abstract class AggregateRoot<T> : IEquatable<AggregateRoot<T>>
    where T : notnull, IComparable<T>
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

    public override string? ToString()
    {
        var stringBuilder = new StringBuilder();

        var type = GetType();
        stringBuilder.AppendFormat("Aggregate: {0}\n", type.Name);

        foreach (var prop in type.GetProperties()) 
        {
            stringBuilder.AppendFormat("\t{0}\t\t= {1}\n", prop.Name, prop.GetValue(this));
        }

        return stringBuilder.ToString();
    }
}