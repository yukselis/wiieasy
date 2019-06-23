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
            var repo = Repository<ActionLog>();

            var actionLog = repo.Create();
            actionLog.Operation = operation;
            actionLog.Description = description;
            actionLog.Action = action?.Left(250);
            actionLog.Url = url;
            actionLog.Parameters = parameters;
            actionLog.IpAddress = HttpContext.Current.Request.GetUserHostAddress();
            actionLog.SessionID = HttpContext.Current.Session?.SessionID;
            actionLog.MachineName = Environment.MachineName;
            actionLog.UserCreatedID = userCreatedId;

            InsertEntity(actionLog);
        }

        public void CreateErrorLog(string moduleName, string function, string code, Exception exc, string note)
        {
            var repo = Repository<ErrorLog>();

            var errorLog = repo.Create();
            errorLog.Module = moduleName;
            errorLog.FunctionName = function;
            errorLog.Code = code?.Left(100);
            errorLog.Message = exc.Message?.Left(250);
            errorLog.StackTrace = exc.ToString();
            errorLog.Note = note;
            errorLog.IpAddress = HttpContext.Current.Request.GetUserHostAddress();
            errorLog.SessionID = HttpContext.Current.Session?.SessionID;
            errorLog.MachineName = Environment.MachineName;

            InsertEntity(errorLog);
        }
    }
}