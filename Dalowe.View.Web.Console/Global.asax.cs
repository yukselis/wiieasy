using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Dalowe.View.Web.Framework;

namespace Dalowe.View.Web.Console
{
    public class MvcApplication : HttpApplication
    {
        protected override void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfig.Load((Client)Client);
        }
    }
}