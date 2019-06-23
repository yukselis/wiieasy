using System.Web.Mvc;
using Dalowe.View.Web.Framework.Controllers;
using Dalowe.View.Web.Framework.Models.Base;

namespace Dalowe.View.Web.Framework.Attributes
{
    public class LayoutModelAttribute : ActionFilterAttribute
    {
        private readonly IViewModelFactory _viewModelFactory;

        public LayoutModelAttribute(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;
            if (controller != null) controller.Context = _viewModelFactory.Create<SharedContext>();

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;

            var model = filterContext.Controller.ViewData.Model as LayoutModel;
            if (model != null)
                model.Context = controller != null && controller.Context != null
                    ? controller.Context
                    : _viewModelFactory.Create<SharedContext>();

            base.OnResultExecuting(filterContext);
        }
    }
}