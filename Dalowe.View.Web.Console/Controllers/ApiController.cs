using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arwend.Web.View.Mvc.Attributes;

namespace Dalowe.View.Web.Console.Controllers
{
    [NoCache]
    public class ApiController : Framework.Controllers.BaseController
    {
        public JsonResult Campaigns()
        {
            var list = Client.Services.API.Management.GetCampaigns();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AccessNodes()
        {
            var list = Client.Services.API.Management.GetAccessNodes();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}