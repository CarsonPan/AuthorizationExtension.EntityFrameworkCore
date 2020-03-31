using System;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationExtenison.EntityFrameworkCore
{
    public class StoreOptions
    {
        public StoreOptions(Type resourceType,Type permissionType,Type permissionRoleType,Type permissionUserType)
        {
            ResourceType=resourceType;
            PermissionType=permissionType;
            PermissionRoleType=permissionRoleType;
            PermissionUserType=permissionUserType;
        }
         public Type ResourceType{get;}
         public Type PermissionType{get;}
         public Type PermissionRoleType{get;} 
         public Type PermissionUserType{get;}
         public TableConfiguration SystemResource{get;set;}=new TableConfiguration();
         public TableConfiguration SystemPermission{get;set;}=new TableConfiguration();
         public TableConfiguration SystemPermissionRole{get;set;}=new TableConfiguration();
         public TableConfiguration SystemPermissionUser{get;set;}=new TableConfiguration();
    }
}