using Arwend.Web.View.Mvc.Models.Base;

namespace Dalowe.View.Web.Framework.Models.Base
{
    public class MailModel : BaseModel
    {
        public string Date { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string DomainUrl { get; set; }
    }
}