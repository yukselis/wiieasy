using System;

namespace Dalowe.View.Web.Framework.Models.Base
{
    public abstract class LogModel : EntityModel
    {
        public string IpAddress { get; set; }
        public string SessionID { get; set; }
        public string MachineName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}