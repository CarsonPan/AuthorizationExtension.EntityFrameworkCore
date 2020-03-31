using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AuthorizationExtension.Core;
using AuthorizationExtension.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationExtenison.EntityFrameworkCore.Stores
{
    public class SystemPermissionRoleStore<TDbContext, TPermissionRole> : ISystemPermissionRoleStore<TPermissionRole>
                 where TDbContext : DbContext
                 where TPermissionRole : SystemPermissionRole, new()
    {
        protected readonly TDbContext DbContext;
        protected readonly DbSet<TPermissionRole> Table;
        public SystemPermissionRoleStore(TDbContext dbContext)
        {
            DbContext = dbContext;
            Table = DbContext.Set<TPermissionRole>();
        }

        public async Task<TPermissionRole> CreateAsync(TPermissionRole permissionRole, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Table.Add(permissionRole);
            await DbContext.SaveChangesAsync(cancellationToken);
            return permissionRole;
        }

        public async Task<IEnumerable<TPermissionRole>> CreateAsync(IEnumerable<TPermissionRole> permissionRoles, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Table.AddRange(permissionRoles);
            await DbContext.SaveChangesAsync(cancellationToken);
            return permissionRoles;
        }

        public async Task<bool> DeleteAsync(string permissionId, string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            TPermissionRole permissionRole = await Table.SingleOrDefaultAsync(pr => pr.PermissionId == permissionId && pr.RoleId == roleId, cancellationToken);
            if (permissionRole != null)
            {
                Table.Remove(permissionRole);
                await DbContext.SaveChangesAsync(cancellationToken);
            }
            return true;
        }

        public async Task<bool> DeleteAsync(IEnumerable<TPermissionRole> permissionRoles, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (TPermissionRole permissionRole in permissionRoles)
            {
                DbContext.Entry(permissionRole).State = EntityState.Deleted;
            }
            await DbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<TPermissionRole> FindPermissionRoleAsync(string permissionId, string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Table.SingleOrDefaultAsync(pr => pr.PermissionId == permissionId && pr.RoleId == roleId, cancellationToken);
        }

        public async Task<IEnumerable<TPermissionRole>> GetPermissionRolesByPermissionIdAsync(string permissionId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Table.Where(p => p.PermissionId == permissionId).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TPermissionRole>> GetPermissionRolesByRoleIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Table.Where(p => p.RoleId == roleId).ToListAsync(cancellationToken);
        }

    }
}