using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arwend.Attributes;
using Dalowe.Domain.Base;

namespace Dalowe.Domain.Visa
{
    public class User : Entity
    {
        [Index("IX_UserName", 1, IsUnique = true)]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Password { get; set; }

        [Index("IX_UserEmail", 2, IsUnique = true)]
        [MaxLength(100)]
        public string Email { get; set; }

        public Role Role { get; set; }
    }
}