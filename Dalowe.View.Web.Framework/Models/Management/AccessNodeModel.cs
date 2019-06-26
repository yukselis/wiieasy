using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Arwend.Web.View.Mvc.Attributes;
using Arwend.Web.View.Mvc.Attributes.DataAnnotations;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Framework.Models.Base;

namespace Dalowe.View.Web.Framework.Models.Management
{
    [Table(Title = "Erişim Noktaları", EditUrl = "/Management/AccessNodeDetail")]
    public class AccessNodeModel : EntityModel
    {
        [Required]
        [Display(Name = "Erişim Noktası Adı")]
        [Column(Title = "Erişim Noktası Adı")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        [Column(Title = "Açıklama")]
        public string Description { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        [Column(Title = "Oluşturulma Tarihi")]
        public DateTime DateCreated { get; set; }
    }
}