using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Arwend.Web.Service
{
    [DataContract(IsReference = true)]
    public class ServiceResult
    {
        [DataMember]
        public List<ServiceResultMessage> Messages { get; set; }
        [DataMember]
        public bool HasFailed { get; set; }
        [DataMember]
        public bool IsRetrievedFromCache { get; set; }
        public void Fail()
        {
            this.HasFailed = true;
        }

        public void Fail(List<ServiceResultMessage> Messages)
        {
            this.Messages = Messages;
            this.HasFailed = true;
        }

        public void Fail(string code, string message) {
            this.Messages.Add(new ServiceResultMessage() { Code = code, Description = message, IsError = true });
            this.HasFailed = true;
        }

        public void Fail(Exception ex)
        {
            while (ex.InnerException != null) {
                ex = ex.InnerException;
            }
            this.Messages.Add(new ServiceResultMessage() { Code = "E1", Description = ex.Message + " " + ex.StackTrace, IsError = true });
            this.HasFailed = true;
        }

        public void Fail(Exception ex, bool reportError, string entry, string fileName)
        {
            this.Fail(ex);
            LogsManager.InsertEntry(entry, ex, fileName);
        }
        public ServiceResult() {
            this.Messages = new List<ServiceResultMessage>();
        }
    }
}
