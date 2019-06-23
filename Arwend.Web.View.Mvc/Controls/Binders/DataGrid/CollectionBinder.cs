using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arwend.Web.View.Mvc.Controls.Table;
namespace Arwend.Web.View.Mvc.Controls.Binders.DataGrid
{
	public class CollectionBinder : WebControl
	{
        private IEnumerable oDataSource;
		private object oInnerDataSource;
		private Configuration oConfiguration;
		private Table.Table oTable;
		private Header oHeader;
		private Footer oFooter;
		public string Title { get; set; }
		public bool HasForm { get; set; }
		public bool IsSubmitted {
			get { return this.Request[this.ID + "GridIsSubmitted"] == "on"; }
		}
		public Footer Footer {
			get { return this.oFooter; }
		}
		public Header Header {
			get { return this.oHeader; }
		}
		public Table.Table Table {
			get { return this.oTable; }
		}
		public IEnumerable DataSource {
			get { return this.oDataSource; }
			set {
				this.oDataSource = value;
				this.DataBind();
			}
		}
		public object InnerDataSource {
			get { return this.oInnerDataSource; }
			set { this.oInnerDataSource = value; }
		}
		public Configuration Configuration {
			get {
				if (this.oConfiguration == null)
					this.oConfiguration = this.CreateConfiguration();
				return this.oConfiguration;
			}
		}
		protected virtual Configuration CreateConfiguration()
		{
			return new Configuration(this);
		}
		public override void DataBind()
		{
			IEnumerable DataSourceToBind = this.DataSource;
			this.OnBeforeDataBind(ref DataSourceToBind);

			this.Table.Rows.Clear();
			this.Table.Editable = this.Configuration.Editable;
			if (this.DataSource != null) {
				long TotalCount = 0;
				this.Table.Accessor.Item = DataSourceToBind;
				this.Table.Accessor.MemberName = "Count";
				try {
					TotalCount = Convert.ToInt64(this.Table.Accessor.Value);
				} catch{
					this.Table.Accessor.MemberName = "Length";
					TotalCount = Convert.ToInt64(this.Table.Accessor.Value);
				}
				this.Table.Rows.Clear();
				int n = 0;
				if (this.Table.ShowColumnHeader) {
                    if (this.Table.Rows.Count == 0)
					    this.Table.Rows.Add();
					for (n = 0; n <= this.Table.Columns.Count - 1; n++) {
						this.Table.Rows[0][n].Text = this.Table.Columns[n].MemberName;
					}
				}
				if (TotalCount > 0) {
					n = 1;
					foreach (object Item in DataSourceToBind) {
						this.Table.Rows.Add(Item).Bind();
						this.OnRowBinded(this.Table.Rows.LastRow);
						this.Table.Rows.LastRow.StyleClass += n % 2 > 0 ? " Odd" : " Even";
						n += 1;
					}
				}
				if (this.Configuration.AllowNewRow) {
					this.Table.Rows.Add(this.CreateBlankItem()).Bind();
					this.Table.Rows.LastRow.IsNew = true;

                    Reflection.Accessor Accessor = new Reflection.Accessor();
                    Accessor.Item = this.DataSource;
                    Accessor.MemberName = "Count";
					long x = Convert.ToInt64(Accessor.Value);
					this.OnRowBinded(this.Table.Rows.LastRow);
					this.Table.Rows.LastRow.StyleClass += n % 2 > 0 ? " Odd" : " Even";
				} else if (TotalCount == 0) {
					this.Table.Visible = false;
					this.Footer.Visible = false;
				}
			}
			DataSourceToBind = null;
			this.OnAfterDataBind();
		}
		protected virtual object CreateBlankItem()
		{
			return null;
		}

		protected virtual void OnRowBinded(Row Row)
		{
		}

		protected virtual void OnBeforeDataBind(ref IEnumerable DataSource)
		{
		}

		protected virtual void OnAfterDataBind()
		{
		}
		protected override void OnBeforeRender(System.Web.UI.HtmlTextWriter writer)
		{
			base.OnBeforeRender(writer);
			if (this.HasForm)
				writer.Write("<form method='post' enctype='multipart/form-data'><input type='hidden' name='" + this.ID + "GridIsSubmitted' id='" + this.ID + "GridIsSubmitted' value='1'>");
		}
		protected override void OnAfterRender(HtmlTextWriter writer)
		{
			base.OnAfterRender(writer);
			if (this.HasForm)
				writer.Write("</form>");
		}
		protected virtual void CreateSections()
		{
			this.oHeader = this.CreateHeader();
			this.oTable = new Table.Table();
			this.oFooter = this.CreateFooter();
			this.Controls.Add(this.Header);
			this.Controls.Add(this.Table);
			this.Controls.Add(this.Footer);

			this.Table.IsSubmitted = this.IsSubmitted;
			this.Table.OnRowCreatedDelegateFunction = this.OnTableRowCreated;
		}
		protected virtual Header CreateHeader()
		{
			return new Header(this);
		}
		protected virtual Footer CreateFooter()
		{
			return new Footer(this);
		}
		private void OnTableRowCreated(Table.Row Row)
		{
            if (!this.Configuration.Editable && (Row.Data != null) && !string.IsNullOrEmpty(this.Configuration.EditLink))
            {
                Reflection.Accessor Accessor = new Reflection.Accessor();
                Accessor.Item = Row.Data;
                Accessor.MemberName = "ID";
                Row.OnClick = "document.location.href='" + this.Configuration.EditLink + "/" + Convert.ToString(Accessor.Value) + "'";
            }
		}
        public CollectionBinder(string Title = "")
		{
			this.StyleClass = "Grid";
			this.Title = Title;
			this.CreateSections();
			this.Table.ShowColumnHeader = true;
			this.HasForm = true;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
