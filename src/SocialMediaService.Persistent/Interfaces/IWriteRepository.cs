using SocialMediaService.Domain.Bases;

namespace SocialMediaService.Persistent.Interfaces;

public interface IWriteRepository<T, TKey, TTransaction>
    where T : AggregateRoot<TKey>
    where TKey : notnull, IComparable<TKey>
{
    Task<TTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}