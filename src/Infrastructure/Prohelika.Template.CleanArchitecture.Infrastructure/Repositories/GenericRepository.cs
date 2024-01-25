using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Prohelika.Template.CleanArchitecture.Domain.Entities;
using Prohelika.Template.CleanArchitecture.Domain.Repositories;
using Prohelika.Template.CleanArchitecture.Infrastructure.Data;

namespace Prohelika.Template.CleanArchitecture.Infrastructure.Repositories;

public abstract class GenericRepository<TEntity, TId>(ApplicationDbContext context) : IGenericRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
{
    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await DbSet.AddAsync(entity, cancellationToken);

        return result.Entity;
    }

    public TEntity Update(TEntity entity)
    {
       var result = DbSet.Update(entity);
       
       return result.Entity;
    }

    public TEntity Delete(TEntity entity)
    {
       var result =  DbSet.Remove(entity);
       
       return result.Entity;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.CountAsync(predicate, cancellationToken);
    }
}