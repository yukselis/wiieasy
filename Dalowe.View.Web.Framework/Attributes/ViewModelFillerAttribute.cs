using System.Web.Mvc;
using AutoMapper;
using Dalowe.View.Web.Framework.Models.Base;
using Dalowe.View.Web.Framework.Models.Visa;

namespace Dalowe.View.Web.Framework.Attributes
{
    public class ViewModelFillerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var model = filterContext.Controller.ViewData.Model as LayoutModel;
            if (model == null || model.Context != null) return;

            var application = (HttpApplication)filterContext.HttpContext.ApplicationInstance;
            var client = (Client)application.Client;
            model.Context = new SharedContext { CurrentUser = Mapper.Map<UserModel>(client.CurrentUser) };
        }
    }
}