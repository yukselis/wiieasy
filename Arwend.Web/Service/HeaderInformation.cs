using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arwend.Web.Service
{
    public class HeaderInformation
    {
        private static RequestInfo _Current;
        public static RequestInfo Current
        {
            get
            {
                _Current = _Current ?? new RequestInfo();
                return _Current;
            }
        }
    }
}
