using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
namespace Arwend.Web.View.Mvc.Controls.Table
{
	public class Row : WebControl
	{
		private RowCollection oCollection;
		private object oItem;
		private bool bHasChecked = false;
		public bool Editable { get; set; }
		public int Index { get; set; }
		public bool IsNew { get; set; }
        public string CellStyle { get; set; }
		public bool IsHeaderRow {
			get { return this.Collection.Table.ShowColumnHeader && this.Index == 0; }
		}
		public bool HasChecked {
			get {
				if (!this.bHasChecked && this.Collection.Table.HasCheckboxes) {
					if (this.Request["CB_Row_" + this.Index] == "on") {
						this.bHasChecked = true;
					}
				}
				return this.bHasChecked;
			}
			set { this.bHasChecked = value; }
		}
		public object Data {
			get { return this.oItem; }
			set { this.oItem = value; }
		}
		protected override HtmlTextWriterTag Tag {
			get {
				if (this.Collection.Table.IsTableLayout) {
                    return HtmlTextWriterTag.Tr;
				} else {
					return base.Tag;
				}
			}

			set { }
		}

		public Cell this[int Index] {
			get {
				if (this.Controls.Count - 1 >= Index) {
					return (Cell)this.Controls[Index];
				}
				return null;
			}
		}

		public RowCollection Collection {
			get { return this.oCollection; }
		}
		public void CreateCells()
		{
			this.Controls.Clear();
			foreach (Column Column in this.Collection.Table.Columns) {
				this.Controls.Add(this.CreateCell(Column));
			}
		}

		public virtual void Bind()
		{
		}
		public virtual void ReverseBind()
		{
			foreach (Cell Cell in this.Controls) {
				Cell.ReverseBind();
			}
		}
		protected virtual Cell CreateCell(Column Column)
		{
			return new Cell(this, Column);
		}
		public Row(RowCollection Collection) : this(Collection, null)
		{
		}
		public Row(RowCollection Collection, object Item)
		{
			this.oCollection = Collection;
			this.Data = Item;
			this.CreateCells();
			this.StyleClass = "Row";
			this.Editable = true;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
