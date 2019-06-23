using System.ComponentModel.DataAnnotations;
using Arwend.Web.View.Mvc.Models.Base;
using Arwend.Web.View.Mvc.Models.Base.Interfaces;

namespace Dalowe.View.Web.Framework.Models.Account
{
    public class LoginModel : BaseModel, IReliable
    {
        public LoginModel()
        {
            Captcha = new CaptchaModel();
        }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Lütfen kullanıcı adınızı giriniz!")]
        public string UserName { get; set; }

        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen şifrenizi giriniz!")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        public CaptchaModel Captcha { get; set; }
    }
}