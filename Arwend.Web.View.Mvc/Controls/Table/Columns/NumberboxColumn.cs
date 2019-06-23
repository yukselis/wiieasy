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
	public class NumberboxColumn : Column
	{
		public bool IsInteger { get; set; }
		public new TextBox DataControl {
			get { return (TextBox)base.DataControl; }
		}
		public override void RenderCell(Cell Cell, bool Editable, System.Web.UI.HtmlTextWriter writer)
		{
			this.DataControl.ID = MemberName + "_" + this.Collection.Table.ID + "_" + Cell.Row.Index;
			if (string.IsNullOrEmpty(Cell.Text))
				Cell.Text = Convert.ToString(Cell.DataValue);
			if (Editable) {
				this.DataControl.Text = Cell.Text;
				this.DataControl.RenderControl(writer);
			} else {
				writer.Write(Cell.Text);
			}
		}
		protected override System.Web.UI.Control CreateDataControl()
		{
			return new TextBox();
		}
		public override void ReverseBind(Cell Cell)
		{
			if (Cell.Row.Data != null) {
				this.DataControl.ID = MemberName + "_" + this.Collection.Table.ID + "_" + Cell.Row.Index;
				((TextBox)this.DataControl).Text = this.Request[this.DataControl.ID];
				if (Information.IsNumeric(this.DataControl.Text)) {
					Cell.DataValue = this.DataControl.Text;
				}
			}
		}
		public NumberboxColumn(ColumnCollection Collection) : base(Collection)
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
