using System;

namespace Dalowe.Domain.Base.Interfaces
{
    public interface IDbEntity
    {
        long ID { get; set; }
        Guid RowGuid { get; set; }
        DateTime DateCreated { get; set; }
        long? UserCreatedID { get; set; }
    }
}