using System;
using System.Linq;
using System.Web.Mvc;
using Arwend.Web.Extensions;
using Dalowe.View.Web.Console.Controllers.Base;
using Dalowe.View.Web.Framework;

namespace Dalowe.View.Web.Console.Filters
{
    public class LogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var action = $"{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}.{filterContext.ActionDescriptor.ActionName}";
                var url = filterContext.HttpContext.Request.RawUrl;
                var parameters = filterContext.ActionParameters;

                var controller = filterContext.Controller as BaseController;
                controller?.Client.Services.API.Log.CreateActionLog("LogFilter", "LogFilter.OnActionExecuting", action, url, parameters.ToJson());
            }
            catch (Exception)
            {
                //ignored
            }
        }
    }
}