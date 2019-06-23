using System;
using System.Web;
using System.Web.SessionState;

namespace Arwend.Web.Extensions
{
    public static class HttpSessionStateExtensions
    {
        public static string GetSessionID(this HttpSessionState session)
        {
            string sessionId = string.Empty;
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                sessionId = Arwend.Utility.GenerateRandomPassword(10);
            else
                sessionId = HttpContext.Current.Session.SessionID;
            return sessionId;
        }
    }
}
