using System;
using System.Web;
using Arwend;
using Arwend.Web.Extensions;
using Dalowe.Domain.Log;
using Dalowe.View.Web.Framework.Services.API.Base;

namespace Dalowe.View.Web.Framework.Services.API
{
    public class LogApi : BaseApi
    {
        public LogApi(Facade facade)
            : base(facade)
        {
        }

        public void CreateActionLog(string operation, string description, string action, string url, string parameters, long? userCreatedId = null)
        {
            var actionLog = new ActionLog
            {
                Operation = operation,
                Description = description,
                Action = action?.Left(250),
                Url = url,
                Parameters = parameters,
                IpAddress = HttpContext.Current.Request.GetUserHostAddress(),
                SessionID = HttpContext.Current.Session?.SessionID,
                MachineName = Environment.MachineName,
                UserCreatedID = userCreatedId
            };

            Add(actionLog);
        }

        public void CreateErrorLog(string moduleName, string function, string code, Exception exc, string note)
        {
            var errorLog = new ErrorLog
            {
                Module = moduleName,
                FunctionName = function,
                Code = code?.Left(100),
                Message = exc.Message?.Left(250),
                StackTrace = exc.ToString(),
                Note = note,
                IpAddress = HttpContext.Current.Request.GetUserHostAddress(),
                SessionID = HttpContext.Current.Session?.SessionID,
                MachineName = Environment.MachineName
            };

            Add(errorLog);
        }
    }
}