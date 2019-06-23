using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dalowe.View.Web.Console
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "CustomerOffer",
                url: "CustomerOffer/{offerId}",
                defaults: new { controller = "Content", action = "OfferProductList" }
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Error", "error", new { controller = "Home", action = "Error" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Account", action = "LockedLogin" }
            );
        }
    }
}
