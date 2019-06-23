using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
namespace Arwend.Web.View.Mvc.Controls.Table.Columns
{
	public class FileColumn : Column
	{
		public string FileNameMember { get; set; }
		public new FileUpload DataControl {
			get { return (FileUpload)base.DataControl; }
		}
		public override void RenderCell(Cell Cell, bool Editable, System.Web.UI.HtmlTextWriter writer)
		{
			if (Cell.Row.IsNew) {
				this.DataControl.ID = this.Collection.Table.ID + "File_" + Cell.Row.Index;
				this.DataControl.RenderControl(writer);
			} else {
				this.Collection.Table.Accessor.Item = Cell.Row.Data;
				this.Collection.Table.Accessor.MemberName = this.FileNameMember;
				Image Image = new Image();
				Image.ImageUrl = Convert.ToString(this.Collection.Table.Accessor.Value);
				Image.RenderControl(writer);
			}
		}
		public override void ReverseBind(Cell Cell)
		{
			System.Web.HttpPostedFile PostedFile = this.Request.Files[this.Collection.Table.ID + "File_" + Cell.Row.Index];
			if (PostedFile != null) {
				Cell.DataValue = PostedFile;
			}
		}
		protected override System.Web.UI.Control CreateDataControl()
		{
			return new FileUpload();
		}
		public FileColumn(ColumnCollection Collection, string MemberName, string FileNameMember) : base(Collection, MemberName)
		{
			this.FileNameMember = FileNameMember;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
