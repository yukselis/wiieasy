using Arwend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arwend.Web
{
    public class HttpApplication : System.Web.HttpApplication
    {
        protected DateTime beginRequestTime, endRequestTime;

        [ThreadStatic]
        private IClient oClient;

        public virtual IClient Client
        {
            get
            {
                if (oClient == null)
                    oClient = this.CreateClient();
                return oClient;
            }
        }
        protected virtual IClient CreateClient()
        {
            return new Client();
        }
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            this.beginRequestTime = DateTime.Now;
            if (ConfigurationManager.RemoveW3Prefix)
            {
                if (Request.Url.Host.StartsWith("www", StringComparison.InvariantCultureIgnoreCase) && !Request.Url.IsLoopback)
                {
                    UriBuilder builder = new UriBuilder(Request.Url);
                    builder.Host = Request.Url.Host.Replace("www.", "");
                    Response.StatusCode = 301;
                    Response.Status = "301 Moved Permanently";
                    Response.StatusDescription = "Moved Permanently";
                    Response.AddHeader("Location", builder.ToString());
                    Response.End();
                }
            }
        }
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {
            this.endRequestTime = DateTime.Now;
            if (oClient != null) {
                oClient.Disconnect();
                oClient = null;
            }
        }
    }
}
