using System;
using Dalowe.Domain.Base.Interfaces;

namespace Dalowe.Domain.Base
{
    public interface ILogEntity : IDbEntity
    {
        string IpAddress { get; set; }
        string SessionID { get; set; }
        string MachineName { get; set; }
    }
}