using System.ComponentModel.DataAnnotations;

namespace Dalowe.View.Web.Console.Models
{
    public class ChangeStatusModel : SimpleEntityModel
    {
        [Required]
        public bool Status { get; set; }
    }
}