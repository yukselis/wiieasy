using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dalowe.Domain.Base.Interfaces;

namespace Dalowe.Domain.Base
{
    public abstract class DbEntity : IDbEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public Guid RowGuid { get; set; }

        public DateTime DateCreated { get; set; }

        public long? UserCreatedID { get; set; }
    }
}