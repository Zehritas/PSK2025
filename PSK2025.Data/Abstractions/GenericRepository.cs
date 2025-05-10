using PSK2025.Data.Contexts;
using Microsoft.EntityFrameworkCore;


namespace PSK2025.Data.Abstractions;

public abstract class GenericRepository<TEntity>(AppDbContext context)
    where TEntity : class
{
    protected readonly AppDbContext Context = context;

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var model = await Context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);

        if (model is null)
        {
            return false;
        }

        Context.Set<TEntity>().Remove(model);
        return true;
    }

    public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().Skip(skip).Take(take).ToListAsync(cancellationToken);
    }
}

