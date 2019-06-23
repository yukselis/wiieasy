using System.ComponentModel.DataAnnotations;
using Arwend.Web.View.Mvc.Attributes.DataAnnotations;

namespace Dalowe.View.Web.Framework.Models.Base
{
    public class TypeModel : EntityModel
    {
        [Display(Name = "Kod")]
        [Column(Title = "Kod")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        [Column(Title = "Açıklama")]
        public string Description { get; set; }
    }
}