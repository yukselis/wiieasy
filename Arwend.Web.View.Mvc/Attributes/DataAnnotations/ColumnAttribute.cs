using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Arwend.Web.View.Mvc.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute()
        {
            this.Sortable = true;
            this.Searchable = true;
            this.Visible = true;
            this.Identity = false;
            this.PreviewAs = PreviewType.Text;
            this.Index = 0;
        }
        public string Title { get; set; }
        public string Width { get; set; }
        public bool Identity { get; set; }
        public bool HashKey { get; set; }
        public bool RangeKey { get; set; }
        public bool Visible { get; set; }
        public bool Sortable { get; set; }
        public bool Searchable { get; set; }
        public int Index { get; set; }
        public PreviewType PreviewAs { get; set; }
    }

    public enum PreviewType : int
    {
        Text = 0,
        Icon = 1,
        SwitchBar = 2,
        Link = 3,
        Image = 4
    }
}
