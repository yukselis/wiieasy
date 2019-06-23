using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arwend.Web.Application.Server.Caching
{
    public enum CachingType : int
    {
        HttpContextCache = 0,
        RuntimeCache = 1,
        Memcached = 2,
        Redis = 3
    }
}
