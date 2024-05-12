using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PR2.Shared.Common;
using SocialMediaService.Domain.Bases;
using SocialMediaService.Persistent.Data;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Persistent.Repositories;

public class EfRepository<T, TKey> : IReadRepository<T, TKey>, IWriteRepository<T, TKey>
    where T : AggregateRoot<TKey>
    where TKey : notnull, IComparable<TKey>
{
    private readonly ApplicationDbContext _context;

    public EfRepository(ApplicationDbContext context) => _context = context;

    protected IQueryable<T> Queryable => _context.Set<T>();

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.BeginTransactionAsync(cancellationToken);

    public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => await _context.Set<T>().FindAsync(id, cancellationToken);

    public virtual IAsyncEnumerable<T> ListAsync(CancellationToken cancellationToken = default)
        => Queryable.AsNoTracking().AsAsyncEnumerable();

    public virtual async Task<Page<T>> GetPageAsync(PageRequest<T> request,
        CancellationToken cancellationToken = default)
    {
        // SELECT * FROM table WHERE ...
        var query = Queryable
            .AsNoTracking()
            .Where(request.Predicate ?? (_ => true));

        // ORDER BY ...
        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        // LIMIT ... OFFSET ...
        query = orderQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        var total = await CountAsync(request.Predicate ?? (_ => true), cancellationToken);

        return new (query.ToList(), total);
    }

    public virtual Task<int> CountAsync(CancellationToken cancellationToken = default) 
        => Queryable.AsNoTracking().CountAsync(cancellationToken);

    public virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => Queryable.AsNoTracking().CountAsync(predicate, cancellationToken);

    public virtual Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        => Queryable.AsNoTracking().AnyAsync(cancellationToken);

    public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => Queryable.AsNoTracking().AnyAsync(predicate, cancellationToken);

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Add(entity);
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Update(entity);
        return await SaveChangesAsync(cancellationToken) > 0;
    }

    public virtual async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Remove(entity);
        return await SaveChangesAsync(cancellationToken) > 0;
    }

    public virtual async Task<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().RemoveRange(entities);
        return await SaveChangesAsync(cancellationToken);
    }

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}