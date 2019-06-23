using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arwend.Web.View.Mvc.Controls.Tabs
{
    public class TabContainer : Base.Control
    {
        private TabPanelCollection oTabs;
        private bool oAutoSwitchEnabled;
        public bool AutoSwitchEnabled
        {
            get { return this.oAutoSwitchEnabled; }
        }
        public TabPanelCollection Tabs
        {
            get
            {
                if (this.oTabs == null)
                    this.oTabs = new TabPanelCollection(this);
                return this.oTabs;
            }
        }
        public TabPanel AddTab(string headerText, string url, string title)
        {
            return this.Tabs.Add(this.ID, headerText, url, title);
        }
        public TabPanel AddTab(string headerText, string content, string title, bool isSelected)
        {
            return this.AddTab(headerText, content, "", title, isSelected);
        }
        public TabPanel AddTab(string headerText, string content, string url, string title, bool isSelected)
        {
            return this.Tabs.Add(this.ID, headerText, content, url, isSelected, title);
        }
        public TabPanel AddTab(TabPanel tab)
        {
            return this.Tabs.Add(tab);
        }
        //public override void OnBeforeDraw(Ardita.View.Web.Content Content)
        //{
        //    if ((this.Tabs != null) && this.Tabs.Count > 0)
        //    {
        //        Structure.Structure Container = new Structure.Structure(this.ID, 2, 1);
        //        Container.Technique = Ardita.View.Web.LayoutTechnique.Css;
        //        Container.Style.Class = "TabContainer";
        //        Container(0, 0).Style.Class = "TabHeaders";
        //        Container(1, 0).Style.Class = "Tabs";
        //        string TabHeaders = "<ul>";
        //        foreach (TabPanel Tab in Tabs)
        //        {
        //            TabHeaders += "<li " + Tab.IsSelected ? "class=\"Selected\"" : "" + !string.IsNullOrEmpty(Tab.OnClick) ? " onclick=\"" + Tab.OnClick + "" : "" + ">";
        //            if (!string.IsNullOrEmpty(Tab.Url))
        //            {
        //                TabHeaders += "<a href=\"" + Tab.Url + "\">" + Tab.HeaderText + "</a>";
        //            }
        //            else
        //            {
        //                TabHeaders += Tab.HeaderText;
        //            }
        //            TabHeaders += "</li>";
        //            if (AutoSwitchEnabled || Tab.IsSelected)
        //                Container(1, 0).Controls.Add(Tab);
        //        }
        //        TabHeaders += "</ul>";
        //        Container(0, 0).Content.Add(TabHeaders);
        //        Container.Style.Width = this.Style.Width;
        //        Content.Add(Container.Draw());
        //    }
        //}
        //protected override void CustomizeScript()
        //{
        //    base.CustomizeScript(Script);
        //    if (this.AutoSwitchEnabled)
        //    {
        //        if (Script.Manager.Script("TabContainerDefaultEvents") == null)
        //        {
        //            ServerSide.ScriptManager.Script TabContainerDefaultEvents = this.Script.Manager.Add("TabContainerDefaultEvents");
        //            TabContainerDefaultEvents.AppendLine("$(document).ready(function() {" + "$(\".TabHeaders li\").click(function(){" + "if(!$(this).hasClass(\"Selected\")) {var CurrentTabIndex = $(\".TabHeaders li\").index(this);" + "$(\".Selected\").removeClass(\"Selected\");" + "$(this).addClass(\"Selected\");" + "$(\".TabPanel:eq(\" + CurrentTabIndex + \")\").addClass(\"Selected\");}" + "});});");
        //        }
        //    }
        //}
        public TabContainer(string MemberName, bool AutoSwitchEnabled = false)
        {
            this.ID = MemberName + "_TabContainer";
            this.oAutoSwitchEnabled = AutoSwitchEnabled;
        }
    }
}
