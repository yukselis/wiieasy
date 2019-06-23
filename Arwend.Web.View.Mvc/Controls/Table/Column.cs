using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Arwend.Web.View.Mvc.Controls.Table
{
	public class Column : WebControl
	{
		private ColumnCollection oCollection;
		protected System.Web.UI.Control oDataControl;
		public string MemberName { get; set; }
		public bool Editable { get; set; }
        public bool AllowSorting { get; set; }
		public int Index { get; set; }
		public override string ID {
			get { return base.ID; }
			set {
				base.ID = value;
				this.MemberName = ID;
			}
		}
		public System.Web.UI.Control DataControl {
			get {
				if (this.oDataControl == null) {
					this.oDataControl = this.CreateDataControl();
				}
				return this.oDataControl;
			}
		}
		public ColumnCollection Collection {
			get { return this.oCollection; }
		}

		public virtual void ReverseBind(Cell Cell)
		{
		}

		public virtual void RenderCell(Cell Cell, bool Editable, HtmlTextWriter writer)
		{
		}
		protected virtual System.Web.UI.Control CreateDataControl()
		{
			return null;
		}
		public Column(ColumnCollection Collection)
		{
			this.oCollection = Collection;
			this.StyleClass = "Column";
			this.Editable = true;
            this.AllowSorting = true;
		}
		public Column(ColumnCollection Collection, string MemberName) : this(Collection)
		{
			this.MemberName = MemberName;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
