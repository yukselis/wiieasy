using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arwend.Web.View.Mvc.Controls.Tabs
{
    public class TabPanelCollection : List<TabPanel>
    {
        private TabContainer oTabContainer;
        public TabContainer TabContainer
        {
            get { return this.oTabContainer; }
        }
        public TabPanel Add(string memberName, string headerText, string url, string title)
        {
            TabPanel Tab = new TabPanel(this, memberName, headerText, url, title);
            return this.Add(Tab);
        }
        public TabPanel Add(string memberName, string headerText, string content, string url, bool isSelected, string title = "", string onclick = "")
        {
            TabPanel Tab = new TabPanel(this, memberName, headerText, content, url, isSelected, title, onclick);
            return this.Add(Tab);
        }
        public new TabPanel Add(TabPanel tab)
        {
            this.Add(tab);
            tab.Index = this.Count - 1;
            return tab;
        }
        public TabPanelCollection(TabContainer tabContainer)
        {
            this.oTabContainer = tabContainer;
        }
    }
}
