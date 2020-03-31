using System;
using System.Linq;
using System.Reflection;
using AuthorizationExtension.Models;
using Microsoft.EntityFrameworkCore;
namespace AuthorizationExtenison.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {

        public static void ConfigureAuthorizationDb(this ModelBuilder modelBuilder, StoreOptions storeOptions)
        {
            modelBuilder.Entity(storeOptions.ResourceType, resource =>
            {
                resource.HasKey("Id");
                resource.Property<string>("Id").HasMaxLength(200).IsRequired();
                resource.Property<string>("Name").HasMaxLength(200).IsRequired();
                resource.Property<ResourceType>("ResourceType").IsRequired();
                resource.Property<string>("PermissionId").HasMaxLength(200).IsRequired();
                resource.Property<string>("ParentId").HasMaxLength(200);
                resource.Property<string>("Description").HasMaxLength(300);
                resource.Property<bool>("IsEnabled").IsRequired();
                string name = storeOptions.SystemResource.Name ?? storeOptions.ResourceType.Name;
                string schema = storeOptions.SystemResource.Schema;
                if (string.IsNullOrEmpty(schema))
                {
                    resource.ToTable(name);
                }
                else
                {
                    resource.ToTable(name, schema);
                }
                storeOptions.SystemResource.BuildAction?.Invoke(resource);
            });
            modelBuilder.Entity(storeOptions.PermissionType, permission =>
            {
                permission.HasKey("Id");
                permission.Property<string>("Id").HasMaxLength(200).IsRequired();
                permission.Property<string>("Name").HasMaxLength(200).IsRequired();
                permission.Property<string>("Description").HasMaxLength(300);
                permission.Property<bool>("AllowedAnonymous").IsRequired();
                permission.Property<bool>("AllowedAllRoles").IsRequired();
                string name = storeOptions.SystemPermission.Name ?? storeOptions.PermissionType.Name;
                string schema = storeOptions.SystemPermission.Schema;
                if (string.IsNullOrEmpty(schema))
                {
                    permission.ToTable(name);
                }
                else
                {
                    permission.ToTable(name, schema);
                }
                storeOptions.SystemPermission.BuildAction?.Invoke(permission);
            });

            modelBuilder.Entity(storeOptions.PermissionRoleType, permissionRole =>
            {
                permissionRole.HasKey("PermissionId","RoleId");
                permissionRole.Property<string>("PermissionId").HasMaxLength(200).IsRequired();
                permissionRole.Property<string>("RoleId").HasMaxLength(200).IsRequired();
                string name = storeOptions.SystemPermissionRole.Name ?? storeOptions.PermissionRoleType.Name;
                string schema = storeOptions.SystemPermissionRole.Schema;
                if (string.IsNullOrEmpty(schema))
                {
                    permissionRole.ToTable(name);
                }
                else
                {
                    permissionRole.ToTable(name, schema);
                }
                storeOptions.SystemPermissionRole.BuildAction?.Invoke(permissionRole);
            });

            modelBuilder.Entity(storeOptions.PermissionUserType, permissionUser =>
            {
                permissionUser.HasKey("PermissionId","UserId");
                permissionUser.Property<string>("PermissionId").HasMaxLength(200).IsRequired();
                permissionUser.Property<string>("UserId").HasMaxLength(200).IsRequired();
                string name = storeOptions.SystemPermissionUser.Name ?? storeOptions.PermissionUserType.Name;
                string schema = storeOptions.SystemPermissionUser.Schema;
                if (string.IsNullOrEmpty(schema))
                {
                    permissionUser.ToTable(name);
                }
                else
                {
                    permissionUser.ToTable(name, schema);
                }
                storeOptions.SystemPermissionUser.BuildAction?.Invoke(permissionUser);
            });

        }
    }

}