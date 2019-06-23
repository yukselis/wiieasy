using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Arwend.Web.View.Mvc.Controls.Binders.DataGrid
{
	public class Configuration
	{
		private int _PageSize = 0;
		private bool _Editable;
        private CollectionBinder oDataGrid;
		public string EditLink { get; set; }
		public string EditLinkParams { get; set; }
		public bool AllowDelete { get; set; }
        public CollectionBinder DataGrid
        {
			get { return this.oDataGrid; }
		}
		public int CurrentPageIndex { get; set; }
		public int TotalPage { get; set; }
		public bool EnablePaging { get; set; }
		public bool AllowNewRow { get; set; }
		public bool Editable {
			get { return _Editable; }
			set {
				_Editable = value;
				this.DataGrid.Table.Editable = value;
			}
		}
		public virtual int PageSize {
			get { return this._PageSize; }
			set {
				this._PageSize = value;
				this.EnablePaging = value > 0;
			}
		}
        public Configuration(CollectionBinder Grid)
		{
			this.oDataGrid = Grid;
			this.EnablePaging = false;
			this.CurrentPageIndex = 0;
			this.TotalPage = 0;
			this.Editable = false;
			this.AllowNewRow = true;
			this.AllowDelete = false;
		}
	}
}
