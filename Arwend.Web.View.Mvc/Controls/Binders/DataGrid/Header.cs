using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
namespace Arwend.Web.View.Mvc.Controls.Binders.DataGrid
{
	public class Header : WebControl
	{
        private CollectionBinder oDataGrid;
		private Table.Table oTable;
        public CollectionBinder DataGrid
        {
			get { return this.oDataGrid; }
		}
		public Table.Table Table {
			get { return this.oTable; }
		}
		protected virtual long TotalRecordCount {
			get { return this.DataGrid.Table.Rows.Count; }
		}
		protected virtual long CurrentLoadedRecordCount {
			get { return this.DataGrid.Table.Rows.Count; }
		}
		protected override void OnBeforeRenderChildren(System.Web.UI.HtmlTextWriter writer)
		{
			this.Table.Rows[0][0].Text = this.DataGrid.Title + " (" + this.CurrentLoadedRecordCount + " / " + this.TotalRecordCount + ")";
			this.Table.Rows[0][0].StyleClass += " Title";
			this.Table.Rows[0][1].StyleClass += " Buttons";
		}
        public Header(CollectionBinder Grid)
		{
			this.oDataGrid = Grid;
			this.oTable = new Table.Table(2, 1);
			this.Controls.Add(this.Table);
			this.StyleClass = "Header";
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
