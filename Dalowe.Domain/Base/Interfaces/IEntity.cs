using System;
using Dalowe.Domain.Base.Interfaces;

namespace Dalowe.Domain.Base
{
    public interface IEntity : IDbEntity
    {
        byte StatusID { get; set; }
        DateTime DateModified { get; set; }
        long? UserModifiedID { get; set; }
    }
}