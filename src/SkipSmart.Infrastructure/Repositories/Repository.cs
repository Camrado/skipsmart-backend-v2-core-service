using Microsoft.EntityFrameworkCore;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Infrastructure.Repositories;

public abstract class Repository<T> where T : Entity {
    protected readonly ApplicationDbContext DbContext;
    
    protected Repository(ApplicationDbContext dbContext) {
        DbContext = dbContext;
    }
    
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
        return await DbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public virtual void Add(T entity) {
        DbContext.Add(entity);
    }
}