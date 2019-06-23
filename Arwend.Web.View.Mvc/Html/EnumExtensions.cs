using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Arwend.Web.View.Mvc.Html
{
    public static class EnumExtensions
    {
        public static MvcHtmlString EnumDropDownList<TModel>(this HtmlHelper<TModel> htmlHelper, string name, object modelData, Type typeToDrawEnum, object htmlAttributes)
        {
            var source = Enum.GetValues(typeToDrawEnum);

            var displayAttributeType = typeof(DisplayAttribute);

            var items = new List<SelectListItem>();
            foreach (var value in source)
            {
                FieldInfo field = value.GetType().GetField(value.ToString());

                var attrs = (DisplayAttribute)field.GetCustomAttributes(displayAttributeType, false).FirstOrDefault();
                object underlyingValue = Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()));

                items.Add(new SelectListItem() { Selected = underlyingValue.Equals(modelData), Text = (attrs != null ? attrs.GetName() : value.ToString()), Value = Convert.ToString(underlyingValue) });
            }
            return htmlHelper.DropDownList(name, items, htmlAttributes);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, short>> expression, Type typeToDrawEnum, object htmlAttributes = null)
        {
            short model = 0;
            if (htmlHelper.ViewData.Model != null)
                model = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            return htmlHelper.EnumDropDownList(ExpressionHelper.GetExpressionText(expression), model, typeToDrawEnum, htmlAttributes);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, byte>> expression, Type typeToDrawEnum, object htmlAttributes = null)
        {
            byte model = 0;
            if (htmlHelper.ViewData.Model != null)
                model = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            return htmlHelper.EnumDropDownList(ExpressionHelper.GetExpressionText(expression), model, typeToDrawEnum, htmlAttributes);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, byte?>> expression, Type typeToDrawEnum, object htmlAttributes = null)
        {
            byte? model = 0;
            if (htmlHelper.ViewData.Model != null)
                model = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            return htmlHelper.EnumDropDownList(ExpressionHelper.GetExpressionText(expression), model.GetValueOrDefault(0), typeToDrawEnum, htmlAttributes);
        }

        public static MvcHtmlString EnumDisplayFor<TModel>(this HtmlHelper<TModel> htmlHelper, object selectedValue, Type typeToDrawEnum, object htmlAttributes = null)
        {
            var source = Enum.GetValues(typeToDrawEnum);

            var displayAttributeType = typeof(DisplayAttribute);

            string selectedEnum = "";
            foreach (var value in source)
            {
                FieldInfo field = value.GetType().GetField(value.ToString());

                var attrs = (DisplayAttribute)field.GetCustomAttributes(displayAttributeType, false).FirstOrDefault();
                object underlyingValue = Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()));

                if (Convert.ToString(underlyingValue).Equals(Convert.ToString(selectedValue)))
                    selectedEnum = (attrs != null ? attrs.GetName() : value.ToString());
            }

            return htmlHelper.Label("", selectedEnum, htmlAttributes);
        }
    }
}
