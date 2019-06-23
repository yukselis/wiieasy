using System.ComponentModel.DataAnnotations;
using Arwend.Web.View.Mvc.Models.Base;
using Arwend.Web.View.Mvc.Models.Base.Interfaces;

namespace Dalowe.View.Web.Framework.Models.Account
{
    public class ForgotPasswordModel : BaseModel, IReliable
    {
        public ForgotPasswordModel()
        {
            Captcha = new CaptchaModel();
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-posta Adresi")]
        public string EmailAddress { get; set; }

        public CaptchaModel Captcha { get; set; }
    }
}