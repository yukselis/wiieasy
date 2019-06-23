using System;
using System.Collections.Generic;

namespace Arwend.Web.View.Mvc.Models.Generics
{
    public class GenericListModel<TListItem> : Base.ListModel
    {
        public GenericListModel()
        {
            this.Items = new List<TListItem>();
        }
        public bool AllowNew { get; set; }
        public bool AlowEdit { get; set; }
        public bool HasItems { get { return this.Items != null && this.Items.Count > 0; } }
        public long OfferItemId { get; set; }
        public long OfferId { get; set; }
        public List<TListItem> Items { get; set; }
    }
}
