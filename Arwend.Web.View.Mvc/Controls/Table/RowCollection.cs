using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Arwend.Web.View.Mvc.Controls.Table
{
	public class RowCollection : List<Row>
	{
		private Table oTable;
		public Table Table {
			get { return this.oTable; }
		}
		public new void Insert(int Index, Row Row)
		{
			base.Insert(Index, Row);
			this.Table.Controls.Add(Row);
		}
		public new void RemoveAt(int Index)
		{
			base.RemoveAt(Index);
			this.Table.Controls.RemoveAt(Index);
		}
		public Row Add()
		{
			Row Row = this.Add(new Row(this, null));
			return Row;
		}
		public Row Add(object Item)
		{
			Row Row = this.Add(new Row(this, Item));
			return Row;
		}
		public new Row Add(Row Row)
		{
			base.Add(Row);
			Row.Index = this.Table.Controls.Count;
			this.Table.OnRowCreated(Row);
			this.Table.Controls.Add(Row);
			return Row;
		}
		public Row LastRow {
			get { return this[this.Count - 1]; }
		}
		public Row FirstRow {
			get { return this[0]; }
		}
		public void Bind()
		{
			foreach (Row Row in this) {
				Row.Bind();
			}
		}
		public void ReverseBind()
		{
			foreach (Row Row in this) {
				Row.ReverseBind();
			}
		}
		public new void Clear()
		{
			base.Clear();
			this.Table.Controls.Clear();
		}
		public RowCollection(Table Table)
		{
			this.oTable = Table;
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
