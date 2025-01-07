using Country.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Country.Infrastructure.Repositories
{
    internal abstract class Repository<TEntity>(ApplicationDbContext context) 
        where TEntity : Entity
    {
        public async Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
            await context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id);
    }
}
