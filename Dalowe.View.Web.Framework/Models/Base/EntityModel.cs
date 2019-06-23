using System;
using System.ComponentModel.DataAnnotations;
using Arwend.Web.View.Mvc.Attributes.DataAnnotations;
using Arwend.Web.View.Mvc.Models.Base;
using Arwend.Web.View.Mvc.Models.Base.Interfaces;

namespace Dalowe.View.Web.Framework.Models.Base
{
    public abstract class EntityModel : BaseModel, IAuditable
    {
        public EntityModel()
        {
            StatusID = true;
        }

        [Display(Name = "ID")]
        [Column(Title = "ID", Identity = true, Visible = false, Index = -1)]
        public long ID { get; set; }

        [Display(Name = "Durum")]
        //[Switch("Aktif", "Pasif", DataTextLabel = "I")]
        //[Column(Title = "Durum", Index = 3)]
        public bool StatusID { get; set; }

        public string Action { get; set; }

        public bool IsNew => ID == 0;

        public new SharedContext Context
        {
            get => (SharedContext)base.Context;
            set => base.Context = value;
        }

        public Guid RowGuid { get; set; }
        public AuditModel History { get; set; }

        public void CreateHistory(long userCreatedId, DateTime dateCreated, long userModifiedId, DateTime dateModified)
        {
            if (userCreatedId > 0)
                History = new AuditModel
                {
                    UserCreatedID = userCreatedId,
                    DateCreated = dateCreated,
                    UserModifiedID = userModifiedId,
                    DateModified = dateModified
                };
        }
    }
}