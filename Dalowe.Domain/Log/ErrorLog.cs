using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dalowe.Domain.Base;
using Dalowe.Domain.Visa;

namespace Dalowe.Domain.Log
{
    public class ErrorLog : LogEntity
    {
        public string Module { get; set; }

        public string FunctionName { get; set; }

        [Index]
        [MaxLength(100)]
        public string Code { get; set; }

        [Index]
        [MaxLength(250)]
        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string Note { get; set; }
    }
}