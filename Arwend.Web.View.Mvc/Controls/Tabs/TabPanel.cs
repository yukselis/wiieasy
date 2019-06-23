using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arwend.Web.View.Mvc.Controls.Tabs
{
    public class TabPanel : Base.Control
    {
        private int nIndex;
        private string sMemberName = string.Empty;
        private bool bIsSelected = false;
        private string sHeaderText = string.Empty;
        private string sContentTitle = string.Empty;
        private string sContent = string.Empty;
        private string sOnClick = string.Empty;
        private string sUrl = string.Empty;
        private TabPanelCollection oCollection;
        public int Index
        {
            get { return this.nIndex; }
            set { this.nIndex = value; }
        }
        public string MemberName
        {
            get { return this.sMemberName; }
        }
        public string HeaderText
        {
            get { return this.sHeaderText; }
        }
        public string ContentTitle
        {
            get { return this.sContentTitle; }
        }
        public TabPanelCollection Collection
        {
            get { return this.oCollection; }
        }
        public string Content
        {
            get { return this.sContent; }
            set { this.sContent = value; }
        }
        public bool IsSelected
        {
            get { return this.bIsSelected; }
            set { this.bIsSelected = value; }
        }
        public string OnClick
        {
            get { return this.sOnClick; }
            set { this.sOnClick = value; }
        }
        public string Url
        {
            get { return this.sUrl; }
            set { this.sUrl = value; }
        }
        public TabPanel(TabPanelCollection collection, string memberName, string headerText, string url, string title)
            : this(collection, memberName, headerText, string.Empty, url, false, title, string.Empty)
        {
        }
        public TabPanel(TabPanelCollection collection, string memberName, string headerText, string url, bool isSelected, string title = "", string onclick = "")
            : this(collection, memberName, headerText, string.Empty, url, isSelected, title, onclick)
        {
        }
        public TabPanel(TabPanelCollection collection, string memberName, string headerText, string content, string url, bool isSelected, string title = "", string onclick = "")
        {
            this.oCollection = collection;
            this.sMemberName = memberName;
            this.sHeaderText = headerText;
            this.sContent = content;
            this.sUrl = url;
            this.bIsSelected = isSelected;
            this.sContentTitle = title;
            this.sOnClick = onclick;
        }
        //public override void OnBeforeDraw(Ardita.View.Web.Content Content)
        //{
        //    Content.Add("<div id=\"").Add(this.MemberName).Add("_Tab_").Add(this.Index).Add("\" class=\"TabPanel");
        //    if (this.IsSelected)
        //    {
        //        Content.Add(" Selected");
        //    }
        //    Content.Add("\">");
        //    if (!string.IsNullOrEmpty(this.ContentTitle))
        //    {
        //        Content.Add("<div class='TabTitleBackground'>");
        //        Content.Add("<div class='TabTitle'>").Add("<span>").Add(this.ContentTitle).Add("</span>").Add("</div>");
        //        Content.Add("</div>");
        //    }
        //    Content.Add("<div class='Content'>");
        //    Content.Add(this.Content);
        //    Content.Add("</div>");
        //    Content.Add("</div>");
        //}
    }
}
