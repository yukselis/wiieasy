using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dalowe.Domain.Base;
using Dalowe.Domain.Visa;

namespace Dalowe.Domain.Log
{
    public class ActionLog : LogEntity
    {
        [Index]
        [MaxLength(250)]
        public string Operation { get; set; }

        public string Description { get; set; }

        [Index]
        [MaxLength(250)]
        public string Action { get; set; }

        public string Url { get; set; }

        public string Parameters { get; set; }
    }
}