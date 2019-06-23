using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
namespace Arwend.Web.View.Mvc.Controls.Binders.DataGrid
{
	public class Footer : WebControl
	{
        private CollectionBinder oDataGrid;
        public CollectionBinder DataGrid
        {
			get { return this.oDataGrid; }
		}

		protected override void OnBeforeRenderChildren(System.Web.UI.HtmlTextWriter writer)
		{
		}
        public Footer(CollectionBinder Grid)
		{
			this.oDataGrid = Grid;
			this.StyleClass = "Footer";
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
