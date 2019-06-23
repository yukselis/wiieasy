using System;
using System.Security.Principal;
using System.Text;
using System.Web;
using Arwend;
using Arwend.Web.Application.Client;
using Arwend.Web.Extensions;
using Arwend.Web.Service;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Framework.Services;

namespace Dalowe.View.Web.Framework
{
    public class Client : Arwend.Web.Client
    {
        private Facade _oServices;

        public bool IsAuthenticated => CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.Name);

        public User CurrentUser
        {
            get => (User)Session["CurrentUser"];
            set
            {
                Session["CurrentUser"] = value;
                SetPrinciples();
            }
        }

        public Facade Services => _oServices ?? (_oServices = new Facade(this));

        public User Login(string username, string password, bool rememberUser)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

            if (!IsAuthenticated)
            {
                var user = Services.API.Account.Login(username, password);
                if (!string.IsNullOrEmpty(user?.Name))
                {
                    LoggedInUserCount += 1;
                    UpdateLoginHistory(ref user);
                    CurrentUser = user;
                    ClearAuthenticationCookies();
                    CreateAuthenticationCookies(username, password, rememberUser);
                }

                Session["HttpRequest"] = Request;
                Session["HttpsRequest"] = Request;
            }

            return CurrentUser;
        }

        public void UpdateLoginHistory(ref User user)
        {
            //if (!user.LastVisitDate.HasValue || user.LastVisitDate.Value.Date != DateTime.Now.Date)
            //    user.VisitCount += 1;

            //user.LastVisitDate = DateTime.Now;
            //user.Save();
        }

        public void Logout()
        {
            CurrentUser = null;
            Context.User = null;
            Session.Clear();
            Session.Abandon();
            ClearAuthenticationCookies();
            LoggedInUserCount -= 1;
        }

        public AccessToken CookieAuthorization()
        {
            var authenticationCookies = CookieManager.Get(ApplicationName + "_AuthenticationToken");
            if (!IsAuthenticated && authenticationCookies != null && !string.IsNullOrEmpty(authenticationCookies.Value))
            {
                var userInfo = ValidateAccessToken(authenticationCookies.Value);
                var username = userInfo.Username;
                var password = userInfo.Password;
                if (!string.IsNullOrEmpty(username))
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        var user = Services.API.Account.Login(username, password);
                        if (user != null)
                            CurrentUser = user;
                        else
                            return userInfo;
                    }
                    else
                    {
                        return userInfo;
                    }
                }
                else
                {
                    ClearAuthenticationCookies();
                }
            }
            else
            {
                ClearAuthenticationCookies();
            }

            return null;
        }

        protected void ClearAuthenticationCookies()
        {
            CookieManager.ClearByName(ApplicationName + "_AuthenticationToken");
        }

        protected void CreateAuthenticationCookies(string username, string password, bool rememberUser)
        {
            var authenticationCookies = new HttpCookie(ApplicationName + "_AuthenticationToken")
            {
                Value = CreateAccessToken(username, rememberUser ? password : string.Empty, string.Empty),
                Expires = DateTime.Now.AddDays(365)
            };
            Response.Cookies.Add(authenticationCookies);
        }

        protected void SetPrinciples()
        {
            if (IsAuthenticated)
            {
                var identity = new GenericIdentity(CurrentUser.Name);
                Context.User = new GenericPrincipal(identity, null);
            }
        }

        public string CreateAccessToken(string userName, string password, string deviceId)
        {
            var token = new AccessToken();
            if (HttpContext.Current != null && IsAuthenticated)
                token.Username = CurrentUser.Name;
            token.Guid = Guid.NewGuid();
            token.Username = userName.Encrypt("8b4@2.Y");
            token.Password = password.Encrypt("8b4@2.Y");
            token.DeviceID = deviceId;
            if (Context != null)
            {
                HeaderInformation.Current.SessionID = SessionID;
                HeaderInformation.Current.UserAgent = Context.Request.UserAgent;
                HeaderInformation.Current.IPAddress = UserHostAddress;
            }
            else
            {
                HeaderInformation.Current.SessionID = string.Empty;
                HeaderInformation.Current.UserAgent = string.Empty;
                HeaderInformation.Current.IPAddress = string.Empty;
            }

            token.Application = ApplicationName;
            token.DateCreated = DateTime.Now;
            token.DateExpire = DateTime.Now.AddDays(1);
            var json = token.ToJson().Encrypt("4.89f6kw3N");
            return HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(json));
        }

        public AccessToken ValidateAccessToken(string value)
        {
            try
            {
                var bytes = HttpServerUtility.UrlTokenDecode(value);
                var json = Encoding.UTF8.GetString(bytes).Decrypt("4.89f6kw3N");
                var token = json.FromJson<AccessToken>();
                token.Username = token.Username.Decrypt("8b4@2.Y");
                token.Password = token.Password.Decrypt("8b4@2.Y");
                return token;
            }
            catch
            {
                return null;
            }
        }
    }
}