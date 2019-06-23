using Arwend;

namespace Dalowe.View.Web.Framework.Models.Account
{
    public class LockedLoginModel : LoginModel
    {
        private string sProfileImagePath;
        public string FullName { get; set; }
        public string EmailAddress { get; set; }

        public string ProfileImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(sProfileImagePath))
                    sProfileImagePath = ConfigurationManager.CdnUrl + "/images/default-user.png";
                return sProfileImagePath;
            }
            set => sProfileImagePath = value;
        }
    }
}