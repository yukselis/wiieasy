using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Arwend.Web.View.Mvc.Controls.Table.Columns
{
	public class CheckboxColumn : Column
	{
		public new CheckBox DataControl {
			get { return (CheckBox)base.DataControl; }
		}
		public override void RenderCell(Cell Cell, bool Editable, System.Web.UI.HtmlTextWriter writer)
		{
			this.DataControl.ID = MemberName + "_" + this.Collection.Table.ID + "_" + Cell.Row.Index;
			if (string.IsNullOrEmpty(Cell.Text)) {
				if ((Information.IsNumeric(Cell.DataValue.ToString()) && Convert.ToInt64(Cell.DataValue) > 0) || Convert.ToBoolean(Cell.DataValue)) {
					Cell.Text = "Evet";
				} else {
					Cell.Text = "Hayır";
				}
			}
			if (Editable) {
				if ((Information.IsNumeric(Cell.DataValue.ToString()) && Convert.ToInt64(Cell.DataValue) > 0) || Convert.ToBoolean(Cell.DataValue)) {
					this.DataControl.Checked = true;
				} else {
					this.DataControl.Checked = false;
				}
				this.DataControl.RenderControl(writer);
			} else {
				writer.Write(Cell.Text);
			}
		}
		protected override System.Web.UI.Control CreateDataControl()
		{
			return new CheckBox();
		}
		public override void ReverseBind(Cell Cell)
		{
			if (Cell.Row.Data != null) {
				this.DataControl.ID = MemberName + "_" + this.Collection.Table.ID + "_" + Cell.Row.Index;
				if ((Information.IsNumeric(this.RequestData.ToString()) && Convert.ToInt64(this.RequestData) > 0) || this.RequestData == "on") {
					Cell.DataValue = true;
				} else {
					Cell.DataValue = false;
				}
			}
		}
		public CheckboxColumn(ColumnCollection Collection) : base(Collection)
		{
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
