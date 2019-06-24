using System;
using Arwend.Web;

namespace Dalowe.View.Web.Framework
{
    public class HttpApplication : Arwend.Web.View.Mvc.HttpApplication
    {
        protected override IClient CreateClient()
        {
            return new Client { ApplicationName = GetType().BaseType.Namespace.Replace(".", "_") };
        }

        protected override void Application_BeginRequest(object sender, EventArgs e)
        {
            base.Application_BeginRequest(sender, e);
        }

        protected override void Application_EndRequest(object sender, EventArgs e)
        {
            base.Application_EndRequest(sender, e);
        }
    }
}