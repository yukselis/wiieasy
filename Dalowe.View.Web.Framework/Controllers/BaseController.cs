using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Arwend;
using Dalowe.View.Web.Framework.Models.Base;
using Controller = Arwend.Web.View.Mvc.Controllers.Base.Controller;

namespace Dalowe.View.Web.Framework.Controllers
{
    public abstract class BaseController : Controller
    {
        public new Client Client => (Client)Application.Client;

        public SharedContext Context { get; set; }

        //protected void SaveEntity<TEntity>(TEntity entity) where TEntity : Entity
        //{
        //    if (entity != null)
        //    {
        //        entity.UserCreatedID = Client.CurrentUser.ID;
        //        entity.UserModifiedID = Client.CurrentUser.ID;
        //        var datasource = RepositoryFactory.Current.GetRepositoryFromEntity<TEntity>();
        //        datasource.Add(entity);
        //        datasource.SaveChanges();
        //    }
        //}

        public bool UploadImage(HttpPostedFileBase postedFile, string directoryPath, out string message)
        {
            return UploadFile(postedFile, directoryPath, new[] { "jpg", "png", "jpeg" }, out message);
        }

        public bool UploadFile(HttpPostedFileBase postedFile, string directoryPath, string[] acceptedFileTypes,
            out string message)
        {
            var result = false;
            message = string.Empty;
            if (postedFile != null && postedFile.ContentLength > 0)
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    var extension = Path.GetExtension(postedFile.FileName)?.Replace(".", "");
                    if (!acceptedFileTypes.Any(type =>
                        type.Equals(extension, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        message = "Geçersiz dosya tipi.";
                    }
                    else
                    {
                        message = UploadFileToLocal(postedFile, directoryPath, extension);
                        result = !string.IsNullOrEmpty(message);
                    }
                }
                else
                {
                    message = "Dosya yolu bulunamadı.";
                }
            else
                message = "Dosya bulunamadı.";

            return result;
        }

        private string UploadFileToLocal(HttpPostedFileBase postedFile, string directoryPath, string extension)
        {
            try
            {
                if (!Directory.Exists(HttpContext.Server.MapPath(directoryPath)))
                    Directory.CreateDirectory(HttpContext.Server.MapPath(directoryPath));

                var filePath = DateTime.Now.AsFileName(postedFile.FileName.Replace(extension, "")) + "." + extension;
                postedFile.SaveAs(HttpContext.Server.MapPath(directoryPath) + filePath);
                return directoryPath + filePath;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetImageUrl(string filePath, bool useCdn = true)
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = "/images/no-image.gif";

            if (useCdn && !string.IsNullOrEmpty(ConfigurationManager.CdnUrl))
                return ConfigurationManager.CdnUrl + filePath;

            return filePath;
        }

        protected ActionResult BackToHome()
        {
            return RedirectToAction("Index", "Home");
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return BackToHome();
        }

        public string ToUrlSlug(string value)
        {
            //First to lower case
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        public string GetRequestData(string url)
        {
            try
            {
                using (var wClient = new WebClient())
                {
                    using (var st = wClient.OpenRead(url))
                    {
                        using (var sr = new StreamReader(st))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}