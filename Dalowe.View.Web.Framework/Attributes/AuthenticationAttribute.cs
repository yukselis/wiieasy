using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dalowe.View.Web.Framework.Attributes
{
    public class AuthenticationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var application = (HttpApplication)httpContext.ApplicationInstance;
            var client = (Client)application.Client;
            client.CookieAuthorization();
            return client.IsAuthenticated;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var attributes = filterContext.ActionDescriptor.GetCustomAttributes(true);
            if (!attributes.Any(a => a is AllowAnonymousAttribute))
            {
                var application = (HttpApplication)filterContext.HttpContext.ApplicationInstance;
                var client = (Client)application.Client;
                if (!client.IsAuthenticated)
                {
                    base.OnAuthorization(filterContext);
                    if (!client.IsAuthenticated)
                    {
                        var url = new UrlHelper(filterContext.RequestContext);
                        filterContext.Result = new RedirectResult(url.Action("LockedLogin", "Account",
                            new { returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl }));
                    }
                }
            }
        }
    }
}