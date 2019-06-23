//using System.Web;
//using System.Web.SessionState;
//namespace Arwend.Web
//{
//    public class UrlManager : IHttpModule, IRequiresSessionState
//    {
//        public void Dispose()
//        {
//        }
//        public void Init(System.Web.HttpApplication Application)
//        {
//            Application.BeginRequest += Application_BeginRequest;
//            Application.EndRequest += Application_EndRequest;
//            Application.Error += Application_OnError;
//        }

//        protected virtual void OnError(HttpApplication HttpApplication)
//        {
//        }
//        private void Application_OnError(object Sender, System.EventArgs e)
//        {
//            this.OnError((HttpApplication)Sender);
//        }

//        private void Application_EndRequest(object Sender, System.EventArgs e)
//        {
//        }
//        private void Application_BeginRequest(object Sender, System.EventArgs e)
//        {
//            HttpApplication Application = (HttpApplication)Sender;
//            string OldUrl = Application.Request.Url.ToString();
//            string NewUrl = this.Rewrite(OldUrl);
//            if (NewUrl != string.Empty && !NewUrl.StartsWith("http") && !NewUrl.StartsWith("www"))
//            {
//                Application.Context.RewritePath("~/" + NewUrl, false);
//            }
//        }
//        protected virtual bool CanBeRewrited(string OldUrl)
//        {
//            return true;
//        }
//        public virtual byte GetManagementType()
//        {
//            return UrlManagementType.Custom.ToByte();
//        }
//        protected virtual string Rewrite(string OldUrl)
//        {
//            if (this.GetManagementType() == UrlManagementType.SinglePage.ToByte() && this.CanBeRewrited(OldUrl))
//            {
//                string NewUrl = "Default.aspx?";
//                string[] QueryStrings = OldUrl.Split('?');
//                If OldUrl.Contains("DisplayFile=") OrElse OldUrl.Contains("DisplayImage=") Then NewUrl = "FileHandler.ashx?"
//                for (int i = 0; i <= QueryStrings.Length - 1; i++)
//                {
//                    if (i > 0)
//                    {
//                        if (i > 1)
//                            NewUrl += "&";
//                        NewUrl += QueryStrings[i];
//                    }
//                }
//                return NewUrl;
//            }
//            return string.Empty;
//        }
//    }
//}
