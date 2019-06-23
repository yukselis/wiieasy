using System.Web.Mvc;
using Arwend;
using AutoMapper;
using Dalowe.View.Web.Framework.Models.Visa;

namespace Dalowe.View.Web.Framework.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string GetImageUrl(this UrlHelper urlHelper, string filePath, bool useCdn = true)
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = "images/no-image.gif";

            if (useCdn && !string.IsNullOrEmpty(ConfigurationManager.CdnUrl))
                filePath = ConfigurationManager.CdnUrl + filePath;
            return filePath;
        }

        public static string GetFileUrl(this UrlHelper urlHelper, string filePath, bool useCdn = true)
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = "/";

            if (useCdn && !string.IsNullOrEmpty(ConfigurationManager.CdnUrl))
                filePath = ConfigurationManager.CdnUrl + filePath;
            return filePath;
        }

        public static Client GetClient(this UrlHelper urlHelper)
        {
            var application = (HttpApplication)urlHelper.RequestContext.HttpContext.ApplicationInstance;
            return (Client)application.Client;
        }

        public static UserModel GetCurrentUser(this UrlHelper urlHelper)
        {
            var client = urlHelper.GetClient();
            return Mapper.Map<UserModel>(client.CurrentUser);
        }
    }
}