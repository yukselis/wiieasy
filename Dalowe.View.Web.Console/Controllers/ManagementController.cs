using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arwend.Web.View.Mvc.Models.Generics;
using AutoMapper;
using Dalowe.Domain.Management;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Console.Controllers.Base;
using Dalowe.View.Web.Framework.Models.Management;
using Dalowe.View.Web.Framework.Models.Visa;

namespace Dalowe.View.Web.Console.Controllers
{
    public class ManagementController : BaseController
    {
        public ManagementController()
        {
            ViewBag.Title = "Kampanya & Erişim Noktası Yönetimi - {0}";
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SaveCampaign(CampaignModel model)
        {
            ViewBag.Title = string.Format(ViewBag.Title, "Kampanyalar");
            if (ModelState.IsValid)
                try
                {
                    var existingCampaign = Client.Services.API.Management.GetCampaign(model.ID);
                    if (model.IsNew)
                    {
                        if (existingCampaign != null)
                        {
                            model.AlertMessage.Caption = "Aynı tanımlayıcı değerlere sahip kayıt bulunmaktadır.";
                            model.AlertMessage.Message = "<span>Mevcut kayıt üzerinden devam et : </span><a href='" + Url.Action("CampaignDetail", new { id = existingCampaign.ID }) + "'>" + existingCampaign.Name + "</a>";
                            return View("CampaignDetail", model);
                        }
                        model.CreateHistory(Client.CurrentUser.ID, DateTime.Now, Client.CurrentUser.ID, DateTime.Now);
                        var campaign = Mapper.Map<Campaign>(model);
                        Client.Services.API.Management.SaveCampaign(campaign);
                    }
                    else
                    {
                        if (existingCampaign == null)
                            return RedirectToAction("CampaignList");

                        model.CreateHistory(0, existingCampaign.DateCreated, Client.CurrentUser.ID, DateTime.Now);
                        existingCampaign.Name = model.Name;
                        existingCampaign.Description = model.Description;
                        Client.Services.API.Management.UpdateCampaign(existingCampaign);
                    }


                    return RedirectToAction("CampaignList");
                }
                catch (Exception ex)
                {
                    // TODO: Handle exceptions and log them.
                }
            return RedirectToAction("CampaignList");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SaveAccessNode(AccessNodeModel model)
        {
            ViewBag.Title = string.Format(ViewBag.Title, "Erişim Noktaları");
            if (ModelState.IsValid)
                try
                {
                    var existingAccessNode = Client.Services.API.Management.GetAccessNode(model.ID);
                    if (model.IsNew)
                    {
                        if (existingAccessNode != null)
                        {
                            model.AlertMessage.Caption = "Aynı tanımlayıcı değerlere sahip kayıt bulunmaktadır.";
                            model.AlertMessage.Message = "<span>Mevcut kayıt üzerinden devam et : </span><a href='" + Url.Action("AccessNodeDetail", new { id = existingAccessNode.ID }) + "'>" + existingAccessNode.Name + "</a>";
                            return View("AccessNodeDetail", model);
                        }
                        model.CreateHistory(Client.CurrentUser.ID, DateTime.Now, Client.CurrentUser.ID, DateTime.Now);
                        var accessNode = Mapper.Map<AccessNode>(model);
                        Client.Services.API.Management.SaveAccessNode(accessNode);
                    }
                    else
                    {
                        if (existingAccessNode == null)
                            return RedirectToAction("AccessNodeList");

                        model.CreateHistory(0, existingAccessNode.DateCreated, Client.CurrentUser.ID, DateTime.Now);
                        existingAccessNode.Name = model.Name;
                        existingAccessNode.Description = model.Description;
                        Client.Services.API.Management.UpdateAccessNode(existingAccessNode);
                    }


                    return RedirectToAction("AccessNodeList");
                }
                catch (Exception ex)
                {
                    // TODO: Handle exceptions and log them.
                }
            return RedirectToAction("AccessNodeList");
        }

        public ActionResult CampaignList()
        {
            ViewBag.Title = string.Format(ViewBag.Title, "Kampanyalar");
            var campaigns = Client.Services.API.Management.GetCampaigns();
            var model = new GenericListModel<CampaignModel>();
            if (campaigns != null && campaigns.Count > 0)
                model.Items = Mapper.Map<List<CampaignModel>>(campaigns);
            return View(model);
        }

        public ActionResult CampaignDetail(long id = 0)
        {
            ViewBag.Title = string.Format(ViewBag.Title, "Kampanya Detayı");
            var model = new CampaignModel { ID = id };
            if (!model.IsNew)
            {
                var user = Client.Services.API.Management.GetCampaign(id);
                if (user != null)
                    model = Mapper.Map<CampaignModel>(user);
            }
            return View(model);
        }

        public ActionResult AccessNodeList()
        {
            ViewBag.Title = string.Format(ViewBag.Title, "Erişim Noktaları");
            var accessNodes = Client.Services.API.Management.GetAccessNodes();
            var model = new GenericListModel<AccessNodeModel>();
            if (accessNodes != null && accessNodes.Count > 0)
                model.Items = Mapper.Map<List<AccessNodeModel>>(accessNodes);
            return View(model);
        }

        public ActionResult AccessNodeDetail(long id = 0)
        {
            ViewBag.Title = string.Format(ViewBag.Title, "Erişim Noktası Detayı");
            var model = new AccessNodeModel { ID = id };
            if (!model.IsNew)
            {
                var user = Client.Services.API.Management.GetAccessNode(id);
                if (user != null)
                    model = Mapper.Map<AccessNodeModel>(user);
            }
            return View(model);
        }
    }
}