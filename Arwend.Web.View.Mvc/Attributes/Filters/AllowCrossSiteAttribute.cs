using System;
using System.Web.Mvc;

namespace Arwend.Web.View.Mvc.Attributes
{
    public class AllowCrossSiteAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Response.Headers.Get("Access-Control-Allow-Origin")))
            {
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "Origin, Content-Type, Accept");
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Request-Method", "GET,POST,OPTIONS");
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");

                //filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri.Replace("https", "http").Replace(filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery,""));
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
