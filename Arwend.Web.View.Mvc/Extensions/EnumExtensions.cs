using System;
using System.Web.Mvc;

namespace Arwend.Web.View.Mvc
{
    public static class EnumExtensions
    {
        public static SelectListItem ToSelectListItem(this Enum value, bool isSelected = false)
        {
            return new SelectListItem { Text = value.ToStringValue(), Value = value.GetValue<Int32>(), Selected = isSelected };
        }

        public static string ToClassName(this Enum style, string prefix)
        {
            var styleText = style.ToString()
                .ToDashCase()
                .ToLower(System.Globalization.CultureInfo.InvariantCulture);
            return string.Concat(prefix, styleText); ;
        }
    }
}
