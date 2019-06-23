using System.Collections.Generic;
using Dalowe.Domain.Base;

namespace Dalowe.Domain.Visa
{
    public class Role : Entity
    {
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}