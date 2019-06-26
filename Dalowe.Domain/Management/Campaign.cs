using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dalowe.Domain.Management
{
    public class Campaign : Base.Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
