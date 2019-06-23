using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
namespace Arwend.Web.View.Mvc.Controls.Table.Columns
{
	public class LinkColumn : Column
	{
		private string sURL = "";
		private string sText = "";
		private string sDisplayMemberName = "";
		public string Text {
			get { return this.sText; }
			set { this.sText = value; }
		}
		public string DisplayMemberName {
			get { return this.sDisplayMemberName; }
			set { this.sDisplayMemberName = value; }
		}
		public string URL {
			get { return this.sURL; }
			set { this.sURL = value; }
		}
		public new HyperLink DataControl {
			get { return (HyperLink)base.DataControl; }
		}
		public override void RenderCell(Cell Cell, bool Editable, System.Web.UI.HtmlTextWriter writer)
		{
			if (string.IsNullOrEmpty(this.DisplayMemberName)) {
				this.DataControl.Text = this.Text;
			}
			if (Cell.Row.Data != null && !string.IsNullOrEmpty(this.MemberName)) {
				this.DataControl.NavigateUrl = this.URL;
				if ((Cell.Row.Data != null)) {
					if (!string.IsNullOrEmpty(this.DisplayMemberName)) {
						this.Collection.Table.Accessor.MemberName = this.DisplayMemberName;
						this.DataControl.Text = Convert.ToString(this.Collection.Table.Accessor.Value);
					} else {
						this.DataControl.Text = this.Text;
					}
					if (!string.IsNullOrEmpty(this.MemberName)) {
						this.Collection.Table.Accessor.MemberName = this.MemberName;
						this.DataControl.NavigateUrl = this.URL + "/" + this.Collection.Table.Accessor.Value;
					}
				} else {
					if (!string.IsNullOrEmpty(Cell.Text)) {
						this.DataControl.Text = Cell.Text;
					} else {
						this.DataControl.Text = this.Text;
					}
				}
			}
			this.DataControl.RenderControl(writer);
		}
		protected override System.Web.UI.Control CreateDataControl()
		{
			return new HyperLink();
		}
		public LinkColumn(ColumnCollection Collection) : base(Collection)
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
