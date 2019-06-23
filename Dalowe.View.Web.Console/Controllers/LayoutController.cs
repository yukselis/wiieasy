using System.Web.Mvc;
using AutoMapper;
using Dalowe.View.Web.Console.Controllers.Base;
using Dalowe.View.Web.Framework.Models.Visa;

namespace Dalowe.View.Web.Console.Controllers
{
    public class LayoutController : BaseController
    {
        public ActionResult Header()
        {
            return PartialView();
        }

        public ActionResult Footer()
        {
            return PartialView();
        }

        public ActionResult LeftMenu()
        {
            return PartialView();
        }

        public ActionResult UserProfileMenu()
        {
            var model = Mapper.Map<UserModel>(Client.CurrentUser);
            return PartialView(model);
        }
    }
}