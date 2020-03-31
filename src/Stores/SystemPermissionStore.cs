using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthorizationExtension.Core;
using AuthorizationExtension.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationExtenison.EntityFrameworkCore.Stores
{
    public class SystemPermissionStore<TDbContext,TPermission> : StoreBase<TPermission>, ISystemPermissionStore<TPermission>
                 where TDbContext:DbContext
                 where TPermission:SystemPermission
    {
        public SystemPermissionStore(TDbContext dbContext) 
               : base(dbContext)
        {
        }
    }
}