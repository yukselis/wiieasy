using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dalowe.Domain.Base
{
    public abstract class LogEntity : DbEntity, ILogEntity
    {
        public string IpAddress { get; set; }

        [Index]
        [MaxLength(250)]
        public string SessionID { get; set; }

        public string MachineName { get; set; }
    }
}