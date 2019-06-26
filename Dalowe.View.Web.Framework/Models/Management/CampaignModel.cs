using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Arwend.Web.View.Mvc.Attributes;
using Arwend.Web.View.Mvc.Attributes.DataAnnotations;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Framework.Models.Base;

namespace Dalowe.View.Web.Framework.Models.Management
{
    [Table(Title = "Kampanyalar", EditUrl = "/Management/CampaignDetail")]
    public class CampaignModel : EntityModel
    {
        [Required]
        [Display(Name = "Kampanya Adı")]
        [Column(Title = "Kampanya Adı")]
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