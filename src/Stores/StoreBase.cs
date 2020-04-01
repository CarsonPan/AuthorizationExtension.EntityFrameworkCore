using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AuthorizationExtension.Core;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationExtenison.EntityFrameworkCore.Stores
{
    public abstract class StoreBase<TEntity> : IStoreBase<TEntity>
    where TEntity : class
    {
        protected readonly DbContext DbContext;
        protected readonly DbSet<TEntity> Table;
        protected StoreBase(DbContext dbContext)
        {
            DbContext = dbContext;
            Table = DbContext.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await Table.AddAsync(entity, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Table.Remove(entity);
            int count = await DbContext.SaveChangesAsync(cancellationToken);
            return count > 0;
        }

        public Task<TEntity> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Table.FindAsync(new object[]{ id}, cancellationToken).AsTask();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Table.Update(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IEnumerable<TEntity> result = Table.Where(filter).AsNoTracking();
            return Task.FromResult(result);
        }
    }
}