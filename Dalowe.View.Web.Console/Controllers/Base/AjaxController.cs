using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Arwend;
using Arwend.Web.Extensions;
using Arwend.Web.View.Mvc.Attributes;
using Dalowe.Domain.Base;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Console.Models;

namespace Dalowe.View.Web.Console.Controllers.Base
{
    [NoCache]
    public class AjaxController : Framework.Controllers.BaseController
    {
        public JsonResult ChangeItemStatus(ChangeStatusModel model)
        {
            var resultMessage = string.Empty;
            Entity item = null;
            if (!ModelState.IsValid)
                resultMessage = "İşlem sırasında hata oluştu!";
            else
                try
                {
                    item = GetEntity(model);
                    if (item != null)
                        switch (model.DataType)
                        {
                            case "User":
                                item.StatusID = (byte)(model.Status ? 1 : 0);
                                item.UserModifiedID = Client.CurrentUser.ID;
                                Client.Services.API.Visa.SaveUser((User)item);
                                break;
                            default:
                                break;
                        }
                }
                catch (Exception ex)
                {
                    resultMessage = "İşlem sırasında hata oluştu!";
                }

            return Json(new { message = resultMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteItem(SimpleEntityModel model)
        {
            var resultMessage = string.Empty;
            Entity item = null;
            if (!ModelState.IsValid)
                resultMessage = "İşlem sırasında hata oluştu!";
            else
                try
                {
                    switch (model.DataType)
                    {
                        case "User":
                            DeleteUser(model.ID);
                            break;
                        case "Campaign":
                            var campaign = Client.Services.API.Management.GetCampaign(model.ID);
                            if (campaign != null)
                                Client.Services.API.Management.DeleteCampaign(campaign);
                            break;
                        case "AccessNode":
                            var accessNode = Client.Services.API.Management.GetAccessNode(model.ID);
                            if (accessNode != null)
                                Client.Services.API.Management.DeleteAccessNode(accessNode);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    resultMessage = "İşlem sırasında hata oluştu!";
                    if (!string.IsNullOrEmpty(ex.Message)) resultMessage = ex.Message;
                }

            return Json(new { message = resultMessage }, JsonRequestBehavior.AllowGet);
        }

        private void DeleteUser(long id)
        {
            var user = Client.Services.API.Visa.GetUser(id);
            if (user != null)
                Client.Services.API.Visa.DeleteUser(user);
        }
        private Entity GetEntity(SimpleEntityModel model)
        {
            Entity entity = null;
            if (ModelState.IsValid)
                try
                {
                    switch (model.DataType)
                    {
                        case "User":
                            entity = Client.Services.API.Visa.GetUser(model.ID);
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                }

            return entity;
        }

        internal dynamic GetBaseData(Entity entity)
        {
            return new
            {
                Status = entity.StatusID,
                UserCreated = entity.UserCreatedID,
                DateCreated = entity.DateCreated.ToTrString("dd MMM yyyy, ddd HH:mm"),
                UserModified = entity.UserModifiedID,
                DateModified = entity.DateModified.ToTrString("dd MMM yyyy, ddd HH:mm")
            };
        }
    }
}