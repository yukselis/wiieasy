using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Arwend.Web.Service
{
    [DataContract(IsReference = true)]
    public class ServiceBooleanResult : ServiceResult
    {
        public ServiceBooleanResult()
        {
            this.Result = false;
            this.HasFailed = false;
        }

        [DataMember]
        public bool Result { get; set; }

        public void Confirm()
        {
            this.Result = true;
            this.HasFailed = false;
        }
    }
}
