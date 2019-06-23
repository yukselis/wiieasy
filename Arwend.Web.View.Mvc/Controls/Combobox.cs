using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Arwend.Reflection;

namespace Arwend.Web.View.Mvc.Controls
{
    public class Combobox : WebControl
    {
        private object oBaseCollection;
        public string MemberName { get; set; }
        public string DisplayMemberName { get; set; }
        public string BaseCollectionMemberName { get; set; }
        public ListType ListType { get; set; }
        private List<ListItem> oItems;
        public object SelectedItem { get; set; }
        public object ParentItem { get; set; }
        private Accessor Accessor = new Accessor();
        private string BlankValue = "$BLANK$";
        private int nHierarhicView = -1;
        public bool Reusable { get; set; }
        public bool HierarchicView
        {
            get
            {
                if (this.nHierarhicView == -1)
                {
                    this.nHierarhicView = 0;
                }
                return this.nHierarhicView > 0;
            }
        }
        protected override HtmlTextWriterTag Tag
        {
            get { return HtmlTextWriterTag.Select; }

            set { }
        }
        public string Text
        {
            get
            {
                if (this.SelectedItem != null)
                {
                    return Convert.ToString(this.SelectedItem);
                }
                return "";
            }
        }
        public List<ListItem> Items
        {
            get
            {
                if (this.oItems == null)
                    this.oItems = new List<ListItem>();
                return this.oItems;
            }
        }
        public object BaseCollection
        {
            get
            {
                if (this.oBaseCollection == null && !string.IsNullOrEmpty(this.BaseCollectionMemberName))
                {
                    this.Accessor.Item = ParentItem;
                    this.Accessor.MemberName = this.BaseCollectionMemberName;
                    this.oBaseCollection = this.Accessor.Value;
                    if (this.oBaseCollection.GetType().IsGenericType)
                    {
                        this.ListType = ListType.GenericList;
                    }
                    else
                    {
                        this.ListType = ListType.Array;
                    }
                }
                return this.oBaseCollection;
            }
            set { this.oBaseCollection = value; }
        }
        public void Bind(bool Submitted = false, bool IsInitial = false)
        {
            if (this.BaseCollection != null)
            {
                this.Binded = true;
                this.Items.Clear();
                this.Items.Add(new ListItem("", this.BlankValue));
                foreach (var Item in (IEnumerable)this.BaseCollection)
                {
                    this.Items.Add(new ListItem(Convert.ToString(Item), Convert.ToString(Item)));
                }
                this.SelectItem(Submitted, IsInitial);
            }
        }
        public void SelectItem(bool Submitted = false, bool IsInitial = false)
        {
            if ((this.ParentItem != null))
            {
                if (this.Reusable)
                {
                    this.Accessor.Item = this.ParentItem;
                    this.Accessor.MemberName = this.MemberName;
                    this.SelectedItem = this.Accessor.Value;
                }
                if ((this.SelectedItem != null))
                {
                    foreach (ListItem ListItem in this.Items)
                    {
                        ListItem.Selected = false;
                        if (!ListItem.Value.Equals(this.BlankValue))
                        {
                            if ((Submitted && this.RequestData == Convert.ToString(ListItem.Value)) || (IsInitial && Convert.ToString(this.SelectedItem) == ListItem.Value))
                            {
                                ListItem.Selected = true;
                            }
                        }
                    }
                }
            }
        }
        public object ReverseBind()
        {
            object SelectedValue = null;
            SelectedValue = this.RequestData;
            this.SelectedItem = SelectedValue;
            return SelectedValue;
        }
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            foreach (ListItem Item in this.Items)
            {
                writer.WriteBeginTag(HtmlTextWriterTag.Option.ToString());
                writer.WriteAttribute(HtmlTextWriterAttribute.Value.ToString(), Item.Value);
                if (Item.Selected)
                    writer.Write("selected");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(Item.Text);
                writer.WriteEndTag(HtmlTextWriterTag.Option.ToString());
            }
        }
        public Combobox(ListType ListType, string MemberName, object ParentItem, object SelectedItem, string DisplayMemberName, object BaseCollection, string BaseCollectionMemberName)
        {
            this.MemberName = MemberName;
            this.ListType = Mvc.Controls.ListType.GenericList;
            this.DisplayMemberName = DisplayMemberName;
            this.BaseCollection = BaseCollection;
            this.BaseCollectionMemberName = BaseCollectionMemberName;
            this.ListType = ListType;
            this.ParentItem = ParentItem;
            this.SelectedItem = SelectedItem;
            this.Reusable = false;
        }
    }
    public enum ListType : int
    {
        GenericList = 1,
        Array = 2
    }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
