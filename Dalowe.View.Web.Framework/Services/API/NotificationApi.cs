using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using Arwend.Net;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Framework.Services.API.Base;

namespace Dalowe.View.Web.Framework.Services.API
{
    public class NotificationApi : BaseApi
    {
        public NotificationApi(Facade facade)
            : base(facade)
        {
        }

        public bool SendMail(string subject, string message, params MailTarget[] targetTypes)
        {
            try
            {
                var target = targetTypes.SelectMany(x => x.SpecialTargets).ToArray();

                var users = Facade.Visa.GetUsers().Where(user => target.Contains(user.ID)).ToList();

                var receipts = users.Select(x => x.Email).ToArray();
                MailingManager.SendMail(receipts, subject, message);
                return true;
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "SendMail", "", exc, "");
                return false;
            }
        }

        public void UserLoggedIn(User user, [CallerFilePath] string callerPath = null, [CallerMemberName] string caller = null, [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                Facade.Log.CreateActionLog("UserLoggedIn", $"{callerPath} : {lineNumber}", caller, HttpContext.Current.Request.RawUrl, user.Name, user.ID);
                SendMail("[EgeMedya] Kullanıcı Girişi", $"{user.Name} ({user.Email}) kullanıcısı sisteme giriş yaptı.", MailTarget.SpecialSales(0));
            }
            catch (Exception exc)
            {
                Facade.Log.CreateErrorLog(ModuleName, "UserLoggedIn", "", exc, "");
            }
        }

        #region MailTarget

        public sealed class MailTarget
        {
            private MailTarget()
            {
            }

            private MailTarget(params long[] specialTargets)
            {
                SpecialTargets = specialTargets;
            }
            
            public long[] SpecialTargets { get; }

            public static MailTarget SpecialSales(params long[] specialTargets)
            {
                return new MailTarget(specialTargets);
            }
        }

        #endregion
    }
}