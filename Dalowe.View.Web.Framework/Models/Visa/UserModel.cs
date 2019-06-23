using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Arwend.Web.View.Mvc.Attributes;
using Arwend.Web.View.Mvc.Attributes.DataAnnotations;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Framework.Models.Base;

namespace Dalowe.View.Web.Framework.Models.Visa
{
    [Table(Title = "Kullanıcılar", EditUrl = "/Visa/UserDetail")]
    public class UserModel : EntityModel
    {
        private string sProfileImagePath;

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        [Column(Title = "Kullanıcı Adı")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "E-posta")]
        [Column(Title = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        public string Password { get; set; }


        [Display(Name = "Oluşturulma Tarihi")]
        [Column(Title = "Oluşturulma Tarihi")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Profil Fotoğrafı")]
        public string ProfileImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(sProfileImagePath))
                    sProfileImagePath = "images/default-user.png";
                return sProfileImagePath;
            }
            set => sProfileImagePath = value;
        }

        [FileSize]
        [FileTypes("png,jpeg,jpg")]
        public HttpPostedFileBase ProfileImage { get; set; }
    }
}