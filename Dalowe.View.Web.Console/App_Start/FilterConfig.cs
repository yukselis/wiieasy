using System.Web;
using System.Web.Mvc;
using Dalowe.View.Web.Console.Filters;

namespace Dalowe.View.Web.Console
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogFilter());
            filters.Add(new ErrorFilter());
        }
    }
}
