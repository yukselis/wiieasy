using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arwend.Reflection;
using Microsoft.VisualBasic;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Arwend.Web.View.Mvc.Controls.Table
{
    public class Table : Controls.WebControl
    {
        private RowCollection oRows;
        private ColumnCollection oColumns;
        private Accessor oAccessor;
        private bool bShowColumnHeader = false;
        public bool Editable { get; set; }
        public delegate void OnRowCreatedDelegate(Row Row);
        public OnRowCreatedDelegate OnRowCreatedDelegateFunction { get; set; }
        public bool HasCheckboxes { get; set; }
        public bool IsSubmitted { get; set; }
        public bool ShowColumnHeader
        {
            get { return this.bShowColumnHeader; }
            set
            {
                if (this.bShowColumnHeader && !value)
                {
                    this.Rows.RemoveAt(0);
                }
                else if (!this.bShowColumnHeader && value)
                {
                    this.Rows.Insert(0, this.Rows.Add());
                }
                this.bShowColumnHeader = value;
            }
        }
        public Accessor Accessor
        {
            get
            {
                if (this.oAccessor == null)
                    this.oAccessor = new Accessor();
                return this.oAccessor;
            }
        }
        public bool IsTableLayout
        {
            get { return base.Tag == HtmlTextWriterTag.Table; }
            set
            {
                if (value)
                {
                    base.Tag = HtmlTextWriterTag.Table;
                }
                else
                {
                    base.Tag = HtmlTextWriterTag.Div;
                }
            }
        }
        public RowCollection Rows
        {
            get
            {
                if (this.oRows == null)
                {
                    this.oRows = this.CreateRowCollection();
                }
                return this.oRows;
            }
        }
        public ColumnCollection Columns
        {
            get
            {
                if (this.oColumns == null)
                {
                    this.oColumns = this.CreateColumnCollection();
                }
                return this.oColumns;
            }
        }
        public Table(int ColumnCount, int RowCount = 0)
            : this()
        {
            for (int n = 0; n <= ColumnCount - 1; n++)
            {
                this.Columns.Add();
            }
            if (this.ShowColumnHeader)
            {
                this.Rows.Add();
            }
            if (RowCount > 0)
            {
                for (int n = 0; n <= RowCount - 1; n++)
                {
                    this.Rows.Add();
                }
            }
        }
        internal virtual void OnRowCreated(Row Row)
        {
            if (this.OnRowCreatedDelegateFunction != null)
                this.OnRowCreatedDelegateFunction(Row);
        }
        public void ReverseBind()
        {
            this.Rows.ReverseBind();
        }
        protected virtual RowCollection CreateRowCollection()
        {
            return new RowCollection(this);
        }
        protected virtual ColumnCollection CreateColumnCollection()
        {
            return new ColumnCollection(this);
        }
        public Table()
        {
            this.StyleClass = "Table";
            this.IsTableLayout = true;
            this.Editable = false;
            this.HasCheckboxes = false;
        }
    }
}
