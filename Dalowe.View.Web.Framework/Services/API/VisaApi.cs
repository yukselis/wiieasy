using System;
using System.Collections.Generic;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Framework.Services.API.Base;

namespace Dalowe.View.Web.Framework.Services.API
{
    public class VisaApi : BaseApi
    {
        public VisaApi(Facade facade)
            : base(facade)
        {
        }

        #region "WriteService Members"

        public void SaveUser(User user)
        {
            Update(user);
        }

        public void DeleteUser(User user)
        {
            Delete(user);
        }

        public void UpdateUser(User user)
        {
            Update(user);
        }
        #endregion

        #region "ReadService Members"

        public User GetUser(long? userId = 0, string username = "", string identityNumber = "", string emailAddress = "",
            string phoneNumber = "")
        {
            User user = null;

            using (var transaction = DbTransaction())
            {
                var repository = transaction.Repository<User>();

                try
                {
                    user = userId > 0 ? repository.Find(u => u.ID == userId, "Role.Permissions") : repository.Find(u => u.Name == username, "Role.Permissions");
                }
                catch (Exception exc)
                {
                    Facade.Log.CreateErrorLog(ModuleName, "GetUser", "", exc, "");
                }
            }

            return user;
        }

        public List<User> GetUsers(int page = 0, int pageSize = 0)
        {
            List<User> users = null;
            try
            {
                users = GetAll<User>();
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "GetUsers", "", exc, "");
            }

            return users;
        }

        public List<Role> GetRoles(int page = 0, int pageSize = 0)
        {
            List<Role> roles = null;
            try
            {
                roles = GetAll<Role>();
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "GetRoles", "", exc, "");
            }

            return roles;
        }

        public List<Permission> GetPermissions(int page = 0, int pageSize = 0)
        {
            List<Permission> permissions = null;
            try
            {
                permissions = GetAll<Permission>();
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "GetPermissions", "", exc, "");
            }

            return permissions;
        }

        public Permission GetPermission(long permissionId)
        {
            Permission permission = null;
            try
            {
                if (permissionId > 0)
                    permission = DbTransaction().Repository<Permission>().Find(x => x.ID == permissionId);
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "GetPermission", "", exc, "");
            }

            return permission;
        }

        public Role GetRole(long roleId)
        {
            Role role = null;
            try
            {
                if (roleId > 0)
                    role = DbTransaction().Repository<Role>().Find(x => x.ID == roleId);
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "GetRole", "", exc, "");
            }

            return role;
        }

        #endregion
    }
}