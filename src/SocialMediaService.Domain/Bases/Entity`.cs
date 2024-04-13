using System.Text;

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

    public override string? ToString()
    {
        var stringBuilder = new StringBuilder();

        var type = GetType();
        stringBuilder.AppendFormat("Entity: {0}\n", type.Name);

        foreach (var prop in type.GetProperties()) 
        {
            stringBuilder.AppendFormat("\t{0}\t\t= {1}\n", prop.Name, prop.GetValue(this));
        }

        return stringBuilder.ToString();
    }
}