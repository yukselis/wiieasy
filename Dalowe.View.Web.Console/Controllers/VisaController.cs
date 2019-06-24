using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Arwend;
using Arwend.Web.View.Mvc.Models.Generics;
using AutoMapper;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Console.Controllers.Base;
using Dalowe.View.Web.Framework.Models.Visa;

namespace Dalowe.View.Web.Console.Controllers
{
    public class VisaController : BaseController
    {
        public VisaController()
        {
            ViewBag.Title = "Kullanıcı & Yetki Yönetimi - {0}";
        }

        public ActionResult UserList()
        {
            ViewBag.Title = string.Format(ViewBag.Title, "Kullanıcılar");
            var users = Client.Services.API.Visa.GetUsers();
            var model = new GenericListModel<UserModel>();
            if (users != null && users.Count > 0)
                model.Items = Mapper.Map<List<UserModel>>(users);
            return View(model);
        }

        public ActionResult UserDetail(long id = 0)
        {
            ViewBag.Title = string.Format(ViewBag.Title, "Kullanıcılar");
            var model = new UserModel { ID = id };
            if (!model.IsNew)
            {
                var user = Client.Services.API.Visa.GetUser(id);
                if (user != null)
                    model = Mapper.Map<UserModel>(user);
            }
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SaveUser(UserModel model)
        {
            ViewBag.Title = string.Format(ViewBag.Title, "Kullanıcılar");
            if (ModelState.IsValid)
                try
                {
                    var existingUser = Client.Services.API.Visa.GetUser(model.ID);
                    if (model.IsNew)
                    {
                        if (existingUser != null)
                        {
                            model.AlertMessage.Caption = "Aynı tanımlayıcı değerlere sahip kayıt bulunmaktadır.";
                            model.AlertMessage.Message = "<span>Mevcut kayıt üzerinden devam et : </span><a href='" + Url.Action("UserDetail", new { id = existingUser.ID }) + "'>" + existingUser.Name + "</a>";
                            return View("UserDetail", model);
                        }
                        model.CreateHistory(Client.CurrentUser.ID, DateTime.Now, Client.CurrentUser.ID, DateTime.Now);
                        var user = Mapper.Map<User>(model);
                        user.Role = Client.Services.API.Visa.GetRole(1);
                        //var password = Utility.GenerateRandomPassword(6);
                        //user.Password = password;
                        //user.LastVisitDate = DateTime.Now;
                        Client.Services.API.Visa.SaveUser(user);
                    }
                    else
                    {
                        if (existingUser == null) return RedirectToAction("UserList");
                        model.CreateHistory(0, existingUser.DateCreated, Client.CurrentUser.ID, DateTime.Now);
                        existingUser.Name = model.Name;
                        existingUser.Email = model.Email;
                        existingUser.Password = model.Password;
                        //user.VisitCount = existingUser.VisitCount;
                        //user.LastVisitDate = existingUser.LastVisitDate;
                        //user.LastVisitDate = DateTime.Now;
                        Client.Services.API.Visa.SaveUser(existingUser);
                    }


                    return RedirectToAction("UserList");
                }
                catch
                {
                    // TODO: Handle exceptions and log them.
                }
            return RedirectToAction("UserList");
        }
    }
}