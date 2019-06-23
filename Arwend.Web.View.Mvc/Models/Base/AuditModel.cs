using System;
using System.ComponentModel.DataAnnotations;

namespace Arwend.Web.View.Mvc.Models.Base
{
    public class AuditModel
    {
        [Display(Name = "Oluşturan Kullanıcı")]
        public string UserCreatedName { get; set; }

        [Display(Name = "Güncelleyen Kullanıcı")]
        public string UserModifiedName { get; set; }

        [Display(Name = "Oluşturan Kullanıcı")]
        public long UserCreatedID { get; set; }

        [Display(Name = "Güncelleyen Kullanıcı")]
        public long UserModifiedID { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Son Güncelleme Tarihi")]
        public DateTime DateModified { get; set; }
    }
}
