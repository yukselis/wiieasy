using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Arwend.Web.View.Mvc.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class TableAttribute : Attribute
    {
        public TableAttribute()
        {
            this.AllowNew = true;
            this.AllowEdit = true;
            this.AllowDelete = true;
            this.AllowSearch = true;
            this.AllowOrder = true;
            this.ShowTableInformation = true;
            this.ShowRowProcess = true;
        }
        public string Title { get; set; }
        public string EditUrl { get; set; }
        public string MemberName { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowNew { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowExpand { get; set; }
        public bool AllowRefresh { get; set; }
        public bool AllowSearch { get; set; }
        public bool AllowOrder { get; set; }
        public bool AllowPaging { get; set; }
        public bool ShowTableInformation { get; set; }
        public bool ShowRowProcess { get; set; }
        public int DefaultOrderColumn { get; set; }
        public string DefaultOrderWay { get; set; }
        public string ItemLayoutRule { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string CellSpacing { get; set; }
        public string CellBordering { get; set; }
        public EditMode EditMode { get; set; }
    }
    public enum EditMode : int
    {
        DetailPage = 0,
        OnRow = 1,
        Popup = 2
    }
}
