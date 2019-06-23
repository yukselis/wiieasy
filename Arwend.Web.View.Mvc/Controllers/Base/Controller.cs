﻿using System.Threading;
namespace Arwend.Web.View.Mvc.Controllers.Base
{
    public abstract class Controller : System.Web.Mvc.Controller
    {
        public Controller()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(ConfigurationManager.Culture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(ConfigurationManager.Culture);
        }
        protected HttpApplication Application { get { return (HttpApplication)this.HttpContext.ApplicationInstance; } }
        protected virtual IClient Client { get { return this.Application.Client; } }
    }
}
