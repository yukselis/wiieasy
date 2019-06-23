using System.Collections.Generic;
using Dalowe.Domain.Base;

namespace Dalowe.Domain.Visa
{
    public enum PermissionType
    {
        Read = 0,
        Write = 1
    }

    public class Permission : Entity
    {
        public string Name { get; set; }

        public PermissionType Type { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}