using System;
using System.Collections.Generic;
using System.Linq;
using Dalowe.Data.Entity;
using Dalowe.Data.Visa;
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
            var datasource = RepositoryFactory.Current.GetRepository<IUserRepository>();
            var item = datasource.GetQuery().First(e => e.ID == user.ID);
            item.Email = user.Email;
            item.StatusID = user.StatusID;
            item.Name = user.Name;
            item.Password = user.Password;
            datasource.SaveChanges();
            RefreshContext();
        }

        #endregion

        #region "ReadService Members"

        public User GetUser(long? userId = 0, string username = "", string identityNumber = "", string emailAddress = "",
            string phoneNumber = "")
        {
            User user = null;
            try
            {
                if (userId > 0)
                    user = RepositoryFactory.Current.GetRepository<IUserRepository>().First(u => u.ID == userId, "Role", "Role.Permissions");
                else
                    user = RepositoryFactory.Current.GetRepository<IUserRepository>().First(u => u.Name == username, "Role", "Role.Permissions");
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "GetUser", "", exc, "");
            }

            return user;
        }

        public List<User> GetUsers(int page = 0, int pageSize = 0)
        {
            List<User> users = null;
            try
            {
                users = RepositoryFactory.Current.GetRepository<IUserRepository>().GetAll().ToList();
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
                roles = RepositoryFactory.Current.GetRepository<IRoleRepository>().GetAll().ToList();
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
                permissions = RepositoryFactory.Current.GetRepository<IPermissionrRepository>().GetAll().ToList();
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
                    permission = RepositoryFactory.Current.GetRepository<IPermissionrRepository>()
                        .Single(x => x.ID == permissionId);
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
                    role = RepositoryFactory.Current.GetRepository<IRoleRepository>().Single(x => x.ID == roleId);
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