using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arwend.Web.View.Mvc.Models.Generics
{
    public class TreeViewModel<TItem> where TItem : class
    {
        public TreeViewModel()
        {
            this.Items = new List<TItem>();
        }
        public IEnumerable<TItem> Items { get; set; }
    }
}
