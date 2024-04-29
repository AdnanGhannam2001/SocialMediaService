using System.Linq.Expressions;
using PR2.Shared.Common;
using SocialMediaService.Domain.Bases;

namespace SocialMediaService.Persistent.Interfaces;

public interface IReadRepository<T, TKey>
    where T : AggregateRoot<TKey>
    where TKey : notnull, IComparable<TKey>
{
    Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    IAsyncEnumerable<T> ListAsync(CancellationToken cancellationToken = default);

    Task<Page<T>> GetPageAsync(PageRequest<T> request,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}