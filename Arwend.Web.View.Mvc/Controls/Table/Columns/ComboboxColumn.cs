using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Arwend.Web.View.Mvc.Controls.Table.Columns
{
	public class ComboboxColumn : Column
	{
		public new Combobox DataControl {
			get {
				if (!((Combobox)base.DataControl).Binded) {
					((Combobox)base.DataControl).Reusable = true;
					((Combobox)base.DataControl).Bind(this.Collection.Table.IsSubmitted, true);
				}
				return (Combobox)base.DataControl;
			}
		}
		public override void RenderCell(Cell Cell, bool Editable, System.Web.UI.HtmlTextWriter writer)
		{
			this.DataControl.ID = MemberName + "_" + this.Collection.Table.ID + "_" + Cell.Row.Index;
			this.DataControl.ParentItem = Cell.Row.Data;
			this.DataControl.SelectItem(this.Collection.Table.IsSubmitted, true);
			if (Editable) {
				this.DataControl.RenderControl(writer);
			} else if (this.DataControl.SelectedItem != null) {
				this.Collection.Table.Accessor.Item = Cell.Row.Data;
				this.Collection.Table.Accessor.MemberName = this.MemberName;

				writer.Write(this.DataControl.SelectedItem);
			}
		}
		public override void ReverseBind(Cell Cell)
		{
			if (Cell.Row.Data != null) {
				this.DataControl.ID = MemberName + "_" + this.Collection.Table.ID + "_" + Cell.Row.Index;
				Cell.DataValue = this.DataControl.ReverseBind();
			}
		}
		public ComboboxColumn(ColumnCollection Collection, string MemberName, List<string> BaseCollection) : base(Collection, MemberName)
		{
			this.oDataControl = new Combobox(ListType.GenericList, MemberName, null, null, "", BaseCollection, "");
		}
		public ComboboxColumn(ColumnCollection Collection, string MemberName, Array BaseCollection) : base(Collection, MemberName)
		{
			this.oDataControl = new Combobox(ListType.Array, MemberName, null, null, "", BaseCollection, "");
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
