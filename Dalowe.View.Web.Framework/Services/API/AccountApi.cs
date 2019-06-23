using System;
using System.Web;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Framework.Services.API.Base;

namespace Dalowe.View.Web.Framework.Services.API
{
    public class AccountApi : BaseApi
    {
        public AccountApi(Facade facade)
            : base(facade)
        {
        }

        public User Login(string username, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    var user = Facade.Visa.GetUser(username: username);
                    if (user != null && password.Equals(user.Password))
                    {
                        Facade.Notification.UserLoggedIn(user);
                        return user;
                    }

                }
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "Login", "", exc, "");
            }

            return null;
        }
    }
}