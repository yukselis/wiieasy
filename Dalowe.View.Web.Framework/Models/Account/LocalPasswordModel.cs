using System.ComponentModel.DataAnnotations;
using Arwend.Web.View.Mvc.Models.Base;
using Arwend.Web.View.Mvc.Models.Base.Interfaces;

namespace Dalowe.View.Web.Framework.Models.Account
{
    public class LocalPasswordModel : BaseModel, IReliable
    {
        public LocalPasswordModel()
        {
            Captcha = new CaptchaModel();
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Şifre")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Şifreniz en az 6 karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre (Tekrar)")]
        [Compare("NewPassword", ErrorMessage = "Girdiğiniz şifreler uyuşmuyor, lütfen tekrar deneyiniz.")]
        public string ConfirmPassword { get; set; }

        public CaptchaModel Captcha { get; set; }
    }
}