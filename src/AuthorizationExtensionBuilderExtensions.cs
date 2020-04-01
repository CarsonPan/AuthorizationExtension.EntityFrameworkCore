using System;
using System.Linq;
using System.Reflection;
using AuthorizationExtenison.EntityFrameworkCore;
using AuthorizationExtenison.EntityFrameworkCore.Stores;
using AuthorizationExtension.Core;
using AuthorizationExtension.Models;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthorizationExtensionBuilderExtensions
    {

        private static void CheckContext(this AuthorizationExtensionBuilder builder, Type contextType)
        {
            PropertyInfo[] properties = contextType.GetProperties();
            Type resourceType=typeof(DbSet<>).MakeGenericType(builder.ResourceType);
            Type permissionType=typeof(DbSet<>).MakeGenericType(builder.PermissionType);
            Type permissionRoleType=typeof(DbSet<>).MakeGenericType(builder.PermissionRoleType);
            Type permissionUserType=typeof(DbSet<>).MakeGenericType(builder.PermissionUserType);
            Type[] types = new Type[] {resourceType,permissionType,permissionRoleType,permissionUserType };
            foreach (Type type in types)
            {
                if (!properties.Any(p =>p.PropertyType==type))
                {
                    
                    throw new InvalidOperationException($" {contextType.Name}类，缺少必要的属性:DbSet<{type.Name}>！");
                }
            }


        }
        public static AuthorizationExtensionBuilder AddEntityFrameworkStores<TContext>(this AuthorizationExtensionBuilder builder, Action<StoreOptions> setupAction=null)
            where TContext : DbContext
        {
            builder.CheckContext(typeof(TContext));
            
            StoreOptions options = new StoreOptions(builder.ResourceType,builder.PermissionType,builder.PermissionRoleType,builder.PermissionUserType);
            setupAction?.Invoke(options);
            builder.Services.AddSingleton(options);

            Type serviceType = typeof(ISystemResourceStore<>).MakeGenericType(builder.ResourceType);
            Type implementationType = typeof(SystemResourceStore<,>).MakeGenericType(typeof(TContext), builder.ResourceType);
            builder.Services.AddScoped(serviceType, implementationType);

            serviceType = typeof(ISystemPermissionStore<>).MakeGenericType(builder.PermissionType);
            implementationType = typeof(SystemPermissionStore<,>).MakeGenericType(typeof(TContext), builder.PermissionType);
            builder.Services.AddScoped(serviceType, implementationType);

            serviceType = typeof(ISystemPermissionRoleStore<>).MakeGenericType( builder.PermissionRoleType);
            implementationType = typeof(SystemPermissionRoleStore<,>).MakeGenericType(typeof(TContext),  builder.PermissionRoleType);
            builder.Services.AddScoped(serviceType, implementationType);

            serviceType = typeof(ISystemPermissionUserStore<>).MakeGenericType(builder.PermissionUserType);
            implementationType = typeof(SystemPermissionUserStore<,>).MakeGenericType(typeof(TContext), builder.PermissionUserType);
            builder.Services.AddScoped(serviceType, implementationType);

            return builder;
        }


    }
}