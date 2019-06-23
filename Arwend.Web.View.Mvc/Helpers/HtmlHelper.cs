using Arwend.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Arwend.Web.View.Mvc.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, object selectedValue, string blankOptionText = "")
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(blankOptionText))
                selectList.Add(new SelectListItem { Value = string.Empty, Text = blankOptionText });
            foreach (Enum item in Enum.GetValues(typeof(TEnum)))
                selectList.Add(item.ToSelectListItem());

            return htmlHelper.DropDownList(name, new SelectList(selectList, "Value", "Text", selectedValue));
        }
        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes = null, string blankOptionText = "")
        {
            //TEnum selectedValue = default(TEnum);
            //TProperty propertyValue = default(TProperty);
            //try
            //{
            //    if (htmlHelper.ViewData.Model != null)
            //        propertyValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            //    if (typeof(TEnum) == typeof(TProperty))
            //        selectedValue = (TEnum)Convert.ChangeType(propertyValue, typeof(TEnum));
            //    else
            //        selectedValue = (TEnum)Enum.Parse(typeof(TEnum), propertyValue.ToString());
            //}
            //catch { }

            var selectList = Enum.GetValues(typeof(TEnum)).OfType<Enum>()
                                                .Select(item => new SelectListItem { Text = item.ToStringValue(), Value = item.ToInt32().ToString() });
            if (!string.IsNullOrEmpty(blankOptionText))
                selectList = (new[] { new SelectListItem { Value = string.Empty, Text = blankOptionText } })
                            .Concat(selectList);

            return htmlHelper.DropDownListFor(expression, new SelectList(selectList, "Value", "Text"), htmlAttributes);
        }
        public static MvcHtmlString EnumCheckBoxListFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, bool sortAlphabetically = true)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var value = metaData.Model;

            var values = Enum.GetValues(typeof(TValue)).Cast<TValue>();

            if (sortAlphabetically)
                values = values.OrderBy(i => i.ToString());

            var builder = new StringBuilder();
            foreach (var item in values)
            {
                TagBuilder tagBuilder = new TagBuilder("input");
                long targetValue = Convert.ToInt64(item);
                long flagValue = Convert.ToInt64(value);

                if ((targetValue & flagValue) == targetValue)
                    tagBuilder.MergeAttribute("checked", "checked");

                tagBuilder.MergeAttribute("type", "checkbox");
                tagBuilder.MergeAttribute("value", item.ToString());
                tagBuilder.MergeAttribute("name", fieldId);

                if (htmlAttributes != null)
                    tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

                tagBuilder.InnerHtml = item.ToString();

                builder.Append(tagBuilder.ToString(TagRenderMode.Normal));
                builder.Append("<br />");
            }

            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString DropDownFor(this HtmlHelper htmlHelper, string name, IEnumerable data, object selectedItem, string displayMember = "Name", string valueMember = "ID")
        {
            var items = new List<SelectListItem>();
            if (data == null) return htmlHelper.DropDownList(name, items);

            items.Add(new SelectListItem() { Selected = false, Text = "Seçiniz", Value = "0" });

            var accessor = new Accessor();
            foreach (var item in data)
            {
                accessor.Item = item;
                accessor.MemberName = displayMember;
                string sText = Convert.ToString(accessor.Value);

                accessor.MemberName = valueMember;
                string sValue = Convert.ToString(accessor.Value);

                bool isSelected = false;
                if (selectedItem != null)
                {
                    if (selectedItem.GetType().IsSubclassOf(typeof(Arwend.Web.View.Mvc.Models.Base.BaseModel)))
                    {
                        accessor.Item = selectedItem;
                        accessor.MemberName = valueMember;
                        isSelected = Convert.ToString(accessor.Value).Equals(sValue);
                    }
                    else
                        isSelected = Convert.ToString(sValue).Equals(selectedItem);
                }

                items.Add(new SelectListItem() { Selected = isSelected, Text = sText, Value = sValue });
            }
            return htmlHelper.DropDownList(name, items);
        }
        public static IDictionary<string, object> MergeHtmlAttributes(this HtmlHelper helper, object htmlAttributesObject, object defaultHtmlAttributesObject)
        {
            var concatKeys = new string[] { "class", "style" };

            var htmlAttributesDict = htmlAttributesObject as IDictionary<string, object>;
            var defaultHtmlAttributesDict = defaultHtmlAttributesObject as IDictionary<string, object>;

            RouteValueDictionary htmlAttributes = (htmlAttributesDict != null)
                ? new RouteValueDictionary(htmlAttributesDict)
                : HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesObject);
            RouteValueDictionary defaultHtmlAttributes = (defaultHtmlAttributesDict != null)
                ? new RouteValueDictionary(defaultHtmlAttributesDict)
                : HtmlHelper.AnonymousObjectToHtmlAttributes(defaultHtmlAttributesObject);

            foreach (var item in htmlAttributes)
            {
                if (concatKeys.Contains(item.Key))
                {
                    defaultHtmlAttributes[item.Key] = (defaultHtmlAttributes[item.Key] != null)
                        ? string.Format("{0} {1}", defaultHtmlAttributes[item.Key], item.Value)
                        : item.Value;
                }
                else
                {
                    defaultHtmlAttributes[item.Key] = item.Value;
                }
            }

            return defaultHtmlAttributes;
        }
    }
}
