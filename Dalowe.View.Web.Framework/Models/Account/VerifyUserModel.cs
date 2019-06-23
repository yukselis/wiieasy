using System.ComponentModel.DataAnnotations;
using Arwend.Web.View.Mvc.Models.Base;

namespace Dalowe.View.Web.Framework.Models.Account
{
    public class VerifyUserModel : BaseModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string ActivationCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre (Tekrar)")]
        [Compare("NewPassword", ErrorMessage = "Girdiğiniz şifreler uyuşmuyor, lütfen tekrar deneyiniz.")]
        public string ConfirmPassword { get; set; }
    }
}