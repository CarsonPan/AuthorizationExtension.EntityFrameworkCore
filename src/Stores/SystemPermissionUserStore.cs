using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AuthorizationExtenison.EntityFrameworkCore;
using AuthorizationExtension.Core;
using AuthorizationExtension.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationExtenison.EntityFrameworkCore.Stores
{
    public class SystemPermissionUserStore<TDbContext, TPermissionUser> : ISystemPermissionUserStore<TPermissionUser>
                 where TDbContext:DbContext
                 where TPermissionUser : SystemPermissionUser, new()
    {
        protected readonly TDbContext DbContext;
        protected readonly DbSet<TPermissionUser> Table;

        public SystemPermissionUserStore(TDbContext dbContext)
        {
            DbContext = dbContext;
            Table = dbContext.Set<TPermissionUser>();
        }

        public async Task<TPermissionUser> CreateAsync(TPermissionUser permissionUser, CancellationToken cancellationToken)
        {
                   cancellationToken.ThrowIfCancellationRequested();
            Table.Add(permissionUser);
            await DbContext.SaveChangesAsync(cancellationToken);
            return permissionUser;
        }

        public async Task<IEnumerable<TPermissionUser>> CreateAsync(IEnumerable<TPermissionUser> permissionUsers, CancellationToken cancellationToken)
        {
                cancellationToken.ThrowIfCancellationRequested();
            Table.AddRange(permissionUsers);
            await DbContext.SaveChangesAsync(cancellationToken);
            return permissionUsers;
        }

        public async Task<bool> DeleteAsync(string permissionId, string userId, CancellationToken cancellationToken)
        {
            TPermissionUser permissionUser= await Table.SingleOrDefaultAsync(pu=>pu.PermissionId==permissionId&&pu.UserId==userId,cancellationToken);
            if(permissionUser!=null)
            {
                Table.Remove(permissionUser);
                await DbContext.SaveChangesAsync(cancellationToken);
            }
            return true;
        }

        public async Task<bool> DeleteAsync(IEnumerable<TPermissionUser> permissionUsers, CancellationToken cancellationToken)
        {
              cancellationToken.ThrowIfCancellationRequested();
            foreach (TPermissionUser permissionUser in permissionUsers)
            {
                DbContext.Entry(permissionUser).State = EntityState.Deleted;
            }
            await DbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<TPermissionUser> FindPermissionUserAsync(string permissionId, string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Table.SingleOrDefaultAsync(pu=>pu.PermissionId==permissionId&&pu.UserId==userId,cancellationToken);
        }

        public async Task<IEnumerable<TPermissionUser>> GetPermissionUsersByPermissionId(string permissionId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Table.Where(pu=>pu.PermissionId==permissionId).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TPermissionUser>> GetPermissionUsersByUserId(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Table.Where(pu=>pu.UserId==userId).ToListAsync(cancellationToken);
        }
        
    }
}