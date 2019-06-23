using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dalowe.Domain.Base
{
    public abstract class Entity : DbEntity, IEntity
    {
        public byte StatusID { get; set; }

        public DateTime DateModified { get; set; }

        public long? UserModifiedID { get; set; }
    }
}