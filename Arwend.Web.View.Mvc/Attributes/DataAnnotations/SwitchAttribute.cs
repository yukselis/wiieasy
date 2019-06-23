using Arwend.Web.View.Mvc.Html;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Arwend.Web.View.Mvc.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwitchAttribute : Attribute
    {
        public SwitchAttribute()
            : this("Açık", "Kapalı")
        {
        }
        public SwitchAttribute(string dataOnLabel, string dataOffLabel)
        {
            this.DataOnLabel = dataOnLabel;
            this.DataOffLabel = dataOffLabel;
            this.DataOn = "danger";
            this.DataOff = "default";
        }
        public string DataOnLabel { get; set; }
        public string DataOffLabel { get; set; }
        public string DataTextLabel { get; set; }
        public AwesomeIcons DataOnIcon { get; set; }
        public AwesomeIcons DataOffIcon { get; set; }
        public AwesomeIcons DataTextIcon { get; set; }
        public string DataOn { get; set; }
        public string DataOff { get; set; }
        public string Size { get; set; }
    }
}
