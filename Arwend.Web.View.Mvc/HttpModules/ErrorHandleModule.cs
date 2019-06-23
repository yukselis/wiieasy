using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Arwend.Web.View.Mvc.Modules
{
        public class ErrorHandleModule : IHttpModule
        {
            private System.Web.HttpApplication context;

            public void Dispose()
            {

            }

            public void Init(System.Web.HttpApplication context)
            {
                this.context = context;
                context.Error += new EventHandler(Application_Error);
            }

            void Application_Error(object sender, EventArgs e)
            {
                var exception = context.Server.GetLastError();
                TryLogException(exception);
                var httpException = exception as HttpException;
                context.Response.Clear();
                context.Server.ClearError();
                var routeData = new RouteData();

                routeData.Values["controller"] = "Error";
                routeData.Values["action"] = "General";
                routeData.Values["exception"] = exception;
                context.Response.StatusCode = 500;

                if (httpException != null)
                {
                    context.Response.StatusCode = httpException.GetHttpCode();
                    switch (context.Response.StatusCode)
                    {
                        case 401:
                            routeData.Values["action"] = "Http401";
                            break;
                        case 403:
                            routeData.Values["action"] = "Http403";
                            break;
                        case 404:
                            routeData.Values["action"] = "Http404";
                            break;
                    }
                }

                IController errorsController = new Controllers.ErrorController();
                var requestedContext = new RequestContext(new HttpContextWrapper(context.Context), routeData);
                errorsController.Execute(requestedContext);
            }

            private void TryLogException(Exception ex)
            {
                try
                {
                   
                }
                catch (Exception)
                {
                    //Do nothing.
                }
            }
        }
}
