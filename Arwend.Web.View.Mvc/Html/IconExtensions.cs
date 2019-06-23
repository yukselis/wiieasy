using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Arwend.Web.View.Mvc.Html
{
    public static class IconExtentions
    {
        public static string Render(this Glyphicons icon, IDictionary<string, object> htmlAttributes, bool inverse = false)
        {
            string className = string.Concat(icon.ToClassName("icon-"), inverse ? " icon-white" : string.Empty);

            var iconBuilder = new TagBuilder("i");
            iconBuilder.MergeAttributes(htmlAttributes);
            iconBuilder.AddCssClass(className);
            return iconBuilder.ToString(TagRenderMode.Normal);
        }
        public static string Render(this AwesomeIcons icon, IDictionary<string, object> htmlAttributes = null)
        {
            string className = "fa " + icon.ToClassName("fa-");

            var iconBuilder = new TagBuilder("i");
            if (htmlAttributes != null)
                iconBuilder.MergeAttributes(htmlAttributes);
            iconBuilder.AddCssClass(className);
            return iconBuilder.ToString(TagRenderMode.Normal);
        }
    }
}
