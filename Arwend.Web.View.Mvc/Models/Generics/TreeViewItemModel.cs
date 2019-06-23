using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arwend.Web.View.Mvc.Models.Generics
{
    public class TreeViewItemModel<TItem> : Base.BaseModel where TItem : class
    {
        public TreeViewItemModel()
            : base()
        {
            this.SubItems = new List<TItem>();
        }

        public TItem Parent { get; set; }
        public IEnumerable<TItem> SubItems { get; set; }
    }
}
