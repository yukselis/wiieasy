using Arwend.Web.View.Mvc.Models.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arwend.Web.View.Mvc.Models.Base
{
    public class TableModel : IPageable, ISortable
    {
        public string Title { get; set; }
        public string EditUrl { get; set; }
        public string MemberName { get; set; }
        public bool Editable { get; set; }
        public bool AllowNew { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowExpand { get; set; }
        public bool AllowRefresh { get; set; }
        public bool AllowSearch { get; set; }
        public bool AllowOrder { get; set; }
        public bool AllowPaging { get; set; }
        public bool ShowTableInformation { get; set; }
        public int DefaultOrderColumn { get; set; }
        public OrderWay DefaultOrderWay { get; set; }
        public string ItemLayoutRule { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string CellSpacing { get; set; }
        public string CellBordering { get; set; }
        public bool StateSave { get; set; }

        public CollocationModel Collocation { get; set; }

        public PaginationModel Pagination { get; set; }
    }
}
