using System.IO;
using System.Web;
using System.Web.Mvc;
using Dalowe.View.Web.Console.Controllers.Base;
using Dalowe.View.Web.Framework;
using HttpApplication = Dalowe.View.Web.Framework.HttpApplication;

namespace Dalowe.View.Web.Console.Filters
{
    public class ErrorFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var function = filterContext.RequestContext.HttpContext.Request.Path;

            string parameters = null;
            try
            {
                using (var stream = filterContext.RequestContext.HttpContext.Request.InputStream)
                {
                    stream.Position = 0;
                    using (var reader = new StreamReader(stream))
                    {
                        parameters = reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                // ignored
            }

            var controller = filterContext.Controller as BaseController;
            controller?.Client.Services.API.Log.CreateErrorLog("ErrorFilter", function, "", filterContext.Exception, parameters);

            filterContext.Result = new RedirectToRouteResult("Error", null);
        }
    }
}