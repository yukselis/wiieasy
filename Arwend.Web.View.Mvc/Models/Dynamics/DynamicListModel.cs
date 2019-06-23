using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arwend.Web.View.Mvc.Models.Dynamics
{
    public class DynamicListModel
    {
        public IEnumerable<DynamicListItemModel> Items { get; set; }
    }
}