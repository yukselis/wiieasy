using Arwend.Web.View.Mvc.Attributes.DataAnnotations;
using Arwend.Web.View.Mvc.Models.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Arwend.Web.View.Mvc.Models.Base
{
    public abstract class ListModel : BaseModel, IPageable, ISortable
    {
        public PaginationModel Pagination { get; set; }
        public CollocationModel Collocation { get; set; }
        public TableModel Table { get; set; }

        public ListModel()
        {
            this.Pagination = new PaginationModel();
            this.Collocation = new CollocationModel();
            if (this.GetType().GetCustomAttributes<TableAttribute>().Any())
            {
                this.Table = new TableModel();
            }
        }
    }
}
