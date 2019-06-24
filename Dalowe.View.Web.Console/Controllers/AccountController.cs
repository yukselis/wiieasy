using System.Web.Mvc;
using AutoMapper;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Console.Controllers.Base;
using Dalowe.View.Web.Console.Models;
using Dalowe.View.Web.Framework.Models.Account;

namespace Dalowe.View.Web.Console.Controllers
{
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Client.IsAuthenticated) return RedirectToLocal(returnUrl);

            var model = new LoginModel { ReturnUrl = returnUrl };
            return View(model);
        }

        /// <summary>
        ///     Kullanıcı bilgilerini doğrulayıp, oturumu başlatır.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Client.Login(model.UserName, model.Password, model.RememberMe);
                if (Client.IsAuthenticated)
                {
                    if (Client.CurrentUser.StatusID != 1)
                    {
                        model.AlertMessage.HasError = true;
                        model.AlertMessage.Message = "Hesabınız pasif durumdadır. Lütfen yöneticinize başvurun.";
                    }
                    return RedirectToLocal(model.ReturnUrl);
                }
                model.AlertMessage.HasError = true;
                model.AlertMessage.Message = "Kullanıcı doğrulanamadı!";
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LockedLogin(string returnUrl)
        {
            if (Client.IsAuthenticated) return BackToHome();

            var userInfo = Client.CookieAuthorization();
            if (string.IsNullOrEmpty(userInfo?.Username))
                return RedirectToAction("Login", "Account", new { returnUrl });

            var user = Client.Services.API.Visa.GetUser(username: userInfo.Username);
            if (user == null)
                return RedirectToAction("Login", "Account", new { returnUrl });

            var model = Mapper.Map<LockedLoginModel>(user);
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        /// <summary>
        ///     Kullanıcı bilgilerini doğrulayıp, oturum başlatır.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult LockedLogin(LockedLoginModel model)
        {
            if (ModelState.IsValid)
            {
                Client.Login(model.UserName, model.Password, model.RememberMe);
                if (Client.IsAuthenticated)
                {
                    if (Client.CurrentUser.StatusID != 1)
                    {
                        model.AlertMessage.HasError = true;
                        model.AlertMessage.Message = "Hesabınız pasif durumdadır. Lütfen yöneticinize başvurun.";
                    }
                    return RedirectToLocal(model.ReturnUrl);
                }
                model.AlertMessage.HasError = true;
                model.AlertMessage.Message = "Kullanıcı doğrulanamadı!";
            }
            return View(model);
        }

        /// <summary>
        ///     Kullanıcı oturumunu sonlandırıp, giriş sayfasına yönlendirir.
        /// </summary>
        public ActionResult LogOut()
        {
            Client.Logout();
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Kullanıcı profil ekranını sunar.
        /// </summary>
        public ActionResult ViewProfile()
        {
            ViewBag.Title = "Profilim";
            var model = Mapper.Map<ViewProfileModel>(Client.CurrentUser);
            return View(model);
        }

        /// <summary>
        ///     Kullanıcı bilgilerini günceller.
        /// </summary>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ViewProfile(ViewProfileModel model)
        {
            if (ModelState.IsValid)
                try
                {
                    //if (model.ProfileImage != null && model.ProfileImage.ContentLength > 0)
                    //{
                    //    if (UploadImage(model.ProfileImage, "/Files/Content/Images/Avatar/", out var profileImagePath))
                    //        model.ProfileImagePath = profileImagePath.Replace("/Files/", "");
                    //}
                    var user = Client.Services.API.Visa.GetUser(model.ID);
                    if (user != null)
                    {
                        user.Name = model.Name;
                        user.Email = model.Email;
                        Client.Services.API.Visa.SaveUser(user);
                    }
                }
                catch
                {
                    // TODO: Handle exceptions and log them.
                }
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
                try
                {
                    var user = Client.CurrentUser;
                    if (user != null && model.OldPassword.Equals(user.Password))
                        user.Password = model.NewPassword;
                }
                catch
                {
                    // TODO: Handle exceptions and log them.
                }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Şifremi unuttum ekranını sunar.
        /// </summary>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            var model = new ForgotPasswordModel();
            return View(model);
        }

        /// <summary>
        ///     Belirtilen email adresi sistemde kayıtlı ise şifre değiştirme linki email adresine yollar ve bilgilendirme mesajı
        ///     görüntüler; sistemde kayıtlı değilse kullanıcıyı uyarır.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public JsonResult ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
                try
                {
                    // TODO: Clear password ve send activation mail.
                }
                catch
                {
                    // TODO: Handle exceptions and log them.
                }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        #endregion
    }
}