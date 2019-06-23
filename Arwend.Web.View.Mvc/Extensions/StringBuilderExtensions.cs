using System;
using System.Text;
using System.Web.Mvc;

namespace Arwend.Web.View.Mvc.Extensions
{
    public static class StringBuilderExtensions
    {
        public static MvcHtmlString ToMvcHtml(this StringBuilder builder)
        {
            return new MvcHtmlString(builder.ToString());
        }
    }
}
