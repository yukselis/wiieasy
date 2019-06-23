using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arwend.Web.View.Mvc
{
    public interface IMappingProvider
    {
        void Load(IClient client);
    }
}
