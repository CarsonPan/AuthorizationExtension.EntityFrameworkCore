using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthorizationExtension.Core;
using AuthorizationExtension.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationExtenison.EntityFrameworkCore.Stores
{
    public class SystemResourceStore<TDbContext, TResource> : StoreBase<TResource>, ISystemResourceStore<TResource>
                 where TDbContext:DbContext
                 where TResource:SystemResource
    {
        public SystemResourceStore(TDbContext dbContext) 
               : base(dbContext)
        {
        }

        public async Task<IEnumerable<TResource>> GetResourcesByPermissionId(string permissionId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Table.Where(r=>r.PermissionId==permissionId).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TResource>> GetResourcesByPermissionIds(IEnumerable<string> permissionIds, CancellationToken cancellationToken)
        {
           cancellationToken.ThrowIfCancellationRequested();
           return await Table.Where(r=>permissionIds.Contains(r.PermissionId)).ToListAsync(cancellationToken);
        }
    }
}