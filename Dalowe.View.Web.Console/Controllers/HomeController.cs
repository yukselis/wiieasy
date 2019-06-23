using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Arwend.Web.View.Mvc.Models.Generics;
using AutoMapper;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Console.Controllers.Base;
using Dalowe.View.Web.Framework.Models.Home;

namespace Dalowe.View.Web.Console.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var model = new IndexModel();

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Help()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return PartialView("_Error");
        }
    }
}