using System.ComponentModel.DataAnnotations;

namespace Dalowe.View.Web.Console.Models
{
    public class SimpleEntityModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string DataType { get; set; }

        public bool DeleteSubItems { get; set; }
    }
}