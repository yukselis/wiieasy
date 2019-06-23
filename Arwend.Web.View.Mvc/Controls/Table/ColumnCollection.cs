using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Arwend.Web.View.Mvc.Controls.Table.Columns;

namespace Arwend.Web.View.Mvc.Controls.Table
{
	public class ColumnCollection : List<Column>
	{
		private Table oTable;
		public Column this[string MemberName] {
			get { return this.Where(Column => !string.IsNullOrEmpty(Column.MemberName) && Column.MemberName.Equals(MemberName)).FirstOrDefault(); }
		}

		public Table Table {
			get { return this.oTable; }
		}
		public ColumnCollection(Table Table)
		{
			this.oTable = Table;
		}
		public Column Add()
		{
			return this.Add(new Column(this));
		}
		public FileColumn AddFileColumn(string MemberName, string ImageLocation, string StyleClass = "")
		{
			FileColumn Column = new FileColumn(this, MemberName, ImageLocation);
			Column.ID = MemberName;
			Column.StyleClass = StyleClass;
            return (FileColumn)this.Add(Column);
		}
		public TextboxColumn AddTextboxColumn(string MemberName, string StyleClass = "")
		{
			TextboxColumn Column = new TextboxColumn(this);
			Column.ID = MemberName;
			Column.StyleClass = StyleClass;
            return (TextboxColumn)this.Add(Column);
		}
		public NumberboxColumn AddNumberboxColumn(string MemberName, string StyleClass = "")
		{
			NumberboxColumn Column = new NumberboxColumn(this);
			Column.ID = MemberName;
			Column.StyleClass = StyleClass;
			return (NumberboxColumn)this.Add(Column);
		}
		public DateTimeColumn AddDateTimeColumn(string MemberName, string StyleClass = "")
		{
			DateTimeColumn Column = new DateTimeColumn(this);
			Column.ID = MemberName;
			Column.StyleClass = StyleClass;
            return (DateTimeColumn)this.Add(Column);
		}
		public FilterboxColumn AddFilterboxColumn(string MemberName, string StyleClass = "")
		{
			FilterboxColumn Column = new FilterboxColumn(this);
			Column.ID = MemberName;
			Column.StyleClass = StyleClass;
            return (FilterboxColumn)this.Add(Column);
		}
		public TextAreaColumn AddTextAreaColumn(string MemberName, string StyleClass = "")
		{
			TextAreaColumn Column = new TextAreaColumn(this);
			Column.ID = MemberName;
			Column.StyleClass = StyleClass;
            return (TextAreaColumn)this.Add(Column);
		}
		public CheckboxColumn AddCheckboxColumn(string MemberName, string StyleClass = "")
		{
			CheckboxColumn Column = new CheckboxColumn(this);
			Column.ID = MemberName;
			Column.StyleClass = StyleClass;
            return (CheckboxColumn)this.Add(Column);
		}
		public LinkColumn AddLinkColumnWithDisplayMemberName(string MemberName, string DisplayMemberName, string URL, string StyleClass = "")
		{
			LinkColumn Column = new LinkColumn(this);
			Column.ID = MemberName;
			Column.StyleClass = StyleClass;
			Column.URL = URL;
			Column.DisplayMemberName = DisplayMemberName;
            return (LinkColumn)this.Add(Column);
		}
		public LinkColumn AddLinkColumnWithText(string ID, string Text, string URL, string StyleClass = "")
		{
			LinkColumn Column = new LinkColumn(this);
			Column.ID = ID;
			Column.MemberName = "";
			Column.StyleClass = StyleClass;
			Column.URL = URL;
			Column.Text = Text;
            return (LinkColumn)this.Add(Column);
		}
		public LinkColumn AddLinkColumn(string MemberName, string Text, string URL, string StyleClass = "")
		{
			LinkColumn Column = new LinkColumn(this);
			Column.ID = MemberName;
			Column.StyleClass = StyleClass;
			Column.URL = URL;
			Column.Text = Text;
			return (LinkColumn)this.Add(Column);
		}
		public LinkColumn AddLinkColumn(string MemberName, string URL, string StyleClass = "")
		{
			LinkColumn Column = new LinkColumn(this);
			Column.ID = MemberName;
			Column.StyleClass = StyleClass;
			Column.URL = URL;
			Column.DisplayMemberName = MemberName;
			return (LinkColumn)this.Add(Column);
		}
		public ComboboxColumn AddComboboxColumn(string MemberName, List<string> BaseCollection)
		{
			ComboboxColumn Column = new ComboboxColumn(this, MemberName, BaseCollection);
			return (ComboboxColumn)this.Add(Column);
		}
		public ComboboxColumn AddComboboxColumn(string MemberName, Array BaseCollection)
		{
			ComboboxColumn Column = new ComboboxColumn(this, MemberName, BaseCollection);
			return (ComboboxColumn)this.Add(Column);
		}
		public new Column Add(Column Column)
		{
			Column.Index = this.Count;
			base.Add(Column);
			return Column;
		}
	}
}
