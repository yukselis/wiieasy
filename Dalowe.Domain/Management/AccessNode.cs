using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalowe.Domain.Base;

namespace Dalowe.Domain.Management
{
    public class AccessNode : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
