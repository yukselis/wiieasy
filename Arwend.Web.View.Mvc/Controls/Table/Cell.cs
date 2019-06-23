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
	public class Cell : WebControl
	{
		private Row oRow;
		private Column oColumn;
		private string sText = "";
		private object oDataValue = null;
		public bool Editable { get; set; }
		public string Prefix { get; set; }
        public override string StyleClass
        {
            get
            {
                string Style = base.StyleClass;
                if(!string.IsNullOrEmpty(this.Row.CellStyle))
                    Style = this.Row.CellStyle + " " + Style;
                if(this.Row.IsHeaderRow && this.Column.AllowSorting)
                    Style = "sorting " + Style;
                return Style;
            }
            set
            {
                base.StyleClass = value;
            }
        }
        public override IDictionary<string, object> HtmlAttributes
        {
            get
            {
                if (this.Row.IsHeaderRow)
                    return this.Column.HtmlAttributes;
                return base.HtmlAttributes;
            }
            set
            {
                base.HtmlAttributes = value;
            }
        }
		public override int Width {
			get { return this.Column.Width; }

			set { }
		}
		public object DataValue {
			get {
				if (this.oDataValue == null) {
					this.SetAccessorProperties();
					this.oDataValue = this.Row.Collection.Table.Accessor.Value;
				}
				return this.oDataValue;
			}
			set {
				this.SetAccessorProperties();
				this.Row.Collection.Table.Accessor.Value = value;
			}
		}
		public Column Column {
			get { return this.oColumn; }
		}
		public Row Row {
			get { return this.oRow; }
		}
		protected override HtmlTextWriterTag Tag {
			get {
				if (this.Row.Collection.Table.IsTableLayout) {
                    if(this.Row.IsHeaderRow)
					    return HtmlTextWriterTag.Th;
                    else
                        return HtmlTextWriterTag.Td;
				} else {
					return base.Tag;
				}
			}

			set { }
		}
		public string Text {
			get { return this.sText; }
			set { this.sText = value; }
		}
		public void ReverseBind()
		{
			bool Editable = false;
			if (this.Column.Collection.Table.Editable) {
				if (this.Row.Editable) {
					if (this.Column.Editable) {
						if (this.Editable) {
							Editable = true;
						}
					}
				}
			}
			if (Editable) {
				this.SetAccessorProperties();
				this.Column.ReverseBind(this);
			}
		}
		protected override void OnAfterWriteContent(HtmlTextWriter writer)
		{
			if (!this.Row.IsHeaderRow) {
				writer.Write(this.Prefix);
				if (this.Row.Collection.Table.HasCheckboxes && this.Column.Index == 0) {
					CheckBox Checkbox = new CheckBox();
					Checkbox.ID = "CB_Row_" + this.Row.Index;
					Checkbox.Checked = this.Row.HasChecked;
					Checkbox.RenderControl(writer);
				}
				bool Editable = false;
				if (this.Column.Collection.Table.Editable) {
					if (this.Row.Editable) {
						if (this.Column.Editable) {
							if (this.Editable) {
								Editable = true;
							}
						}
					}
				}
				if (this.Row.Data != null) {
					this.SetAccessorProperties();
					this.Column.RenderCell(this, Editable, writer);
				} else {
					writer.Write(this.Text);
				}
			} else {
				writer.Write(this.Text);
			}
		}
		private void SetAccessorProperties()
		{
			if (this.Row.Data != null) {
				this.Row.Collection.Table.Accessor.Item = this.Row.Data;
				this.Row.Collection.Table.Accessor.MemberName = this.Column.MemberName;
			}
		}
		public Cell(Row Row, Column Column)
		{
			this.oRow = Row;
			this.oColumn = Column;
			this.StyleClass = "Cell";
			this.Editable = true;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
