using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Arwend;
using Arwend.Attributes;
using Arwend.Reflection;
using Arwend.Web.Extensions;
using Arwend.Web.View.Mvc.Attributes.DataAnnotations;
using Arwend.Web.View.Mvc.Extensions;
using Arwend.Web.View.Mvc.Helpers;
using Arwend.Web.View.Mvc.Html;
using Arwend.Web.View.Mvc.Models;
using Arwend.Web.View.Mvc.Models.Base;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Framework.Models.Base;

namespace Dalowe.View.Web.Framework.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static User GetCurrentUser(this HtmlHelper htmlHelper)
        {
            var application = (HttpApplication)htmlHelper.ViewContext.HttpContext.ApplicationInstance;
            var client = (Client)application.Client;
            return client.CurrentUser;
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList,
            bool showBlankOption, string blankOptionText = "Seçiniz")
        {
            var selectedValue = default(TProperty);
            if (htmlHelper.ViewData.Model != null)
            {
                selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);
                var type = typeof(TModel);
                var property = type.GetProperty(((MemberExpression)expression.Body).Member.Name);
                if (Attribute.IsDefined(property, typeof(ColumnAttribute)))
                {
                    var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                    if (columnAttribute.Identity || columnAttribute.HashKey || columnAttribute.RangeKey)
                    {
                        var model = htmlHelper.ViewData.Model as EntityModel;
                        if (!model.IsNew)
                            return htmlHelper.TextFieldFor(expression);
                    }
                }
            }

            var attributes = new RouteValueDictionary(new { @class = "form-control validate" });
            if (showBlankOption && !string.IsNullOrEmpty(blankOptionText))
                selectList = new[] { new SelectListItem { Value = string.Empty, Text = blankOptionText } }
                    .Concat(selectList);

            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div class='col-md-9'>");
            builder.Append(htmlHelper.DropDownListFor(expression,
                new SelectList(selectList, "Value", "Text", selectedValue), attributes));
            builder.Append(htmlHelper.ValidationMessageFor(expression));
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty, TEnum>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string blankOptionText = "")
        {
            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div class='col-md-9'>");
            builder.Append(htmlHelper.EnumDropDownListFor<TModel, TProperty, TEnum>(expression,
                new RouteValueDictionary(new { @class = "form-control validate col-md-6" }), blankOptionText));
            builder.Append(htmlHelper.ValidationMessageFor(expression));
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString SwitchFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression, SwitchAttribute swithAttribute)
        {
            return htmlHelper.SwitchFor(expression, swithAttribute.DataOn, swithAttribute.DataOff,
                swithAttribute.DataOnLabel, swithAttribute.DataOffLabel, swithAttribute.DataTextLabel,
                swithAttribute.DataOnIcon, swithAttribute.DataOffIcon, swithAttribute.DataTextIcon);
        }

        public static MvcHtmlString SwitchFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression, string dataOn, string dataOff, string dataOnLabel,
            string dataOffLabel, string dataTextLabel, AwesomeIcons dataOnIcon = AwesomeIcons.None,
            AwesomeIcons dataOffIcon = AwesomeIcons.None, AwesomeIcons dataTextIcon = AwesomeIcons.None)
        {
            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div class='col-md-9'>");
            builder.Append("<div data-on='").Append(dataOn).Append("' data-off='").Append(dataOff)
                .Append("' class='make-switch' data-on-label='");

            if (dataOnIcon != AwesomeIcons.None)
                builder.Append(dataOnIcon.Render());
            else
                builder.Append(dataOnLabel);

            builder.Append("' data-off-label='");
            if (dataOffIcon != AwesomeIcons.None)
                builder.Append(dataOffIcon.Render());
            else
                builder.Append(dataOffLabel);

            builder.Append("' data-text-label='");
            if (dataTextIcon != AwesomeIcons.None)
                builder.Append(dataTextIcon.Render());
            else
                builder.Append(dataTextLabel);
            builder.Append("'>");
            builder.Append(htmlHelper.CheckBoxFor(expression));
            builder.Append("</div>");
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }
        //public static MvcHtmlString MenuItemListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string menu, string self = "", bool showBlankOption = false, string blankOptionText = "Seçiniz")
        //{
        //    var controller = htmlHelper.ViewContext.Controller as BaseController;
        //    var menuItemList = controller.Client.Services.API.Content.GetMenuItems(menu, false);
        //    if (!string.IsNullOrEmpty(self))
        //        menuItemList = menuItemList.FindAll(m => m.Code != self);
        //    var selectList = menuItemList.Select(item => new SelectListItem { Value = item.Code, Text = item.Name });

        //    return htmlHelper.DropDownListFor(expression, selectList, true);
        //}
        //public static MvcHtmlString MenuListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string mainMenu = "", bool showBlankOption = false, string blankOptionText = "Seçiniz")
        //{
        //    var controller = htmlHelper.ViewContext.Controller as BaseController;
        //    var menuList = controller.Client.Services.API.Content.GetMenus();
        //    if (!string.IsNullOrEmpty(mainMenu))
        //        menuList = menuList.FindAll(m => m.Code != mainMenu);
        //    var selectList = menuList.Select(menu => new SelectListItem { Value = menu.Code, Text = menu.Title });

        //    return htmlHelper.DropDownListFor(expression, selectList, true);
        //}
        //public static MvcHtmlString LanguagesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int cultureId = 0, bool showBlankOption = false, string blankOptionText = "Seçiniz")
        //{
        //    var controller = htmlHelper.ViewContext.Controller as BaseController;
        //    var selectList = controller.Client.Services.API.Localization.GetLanguages(cultureKey)
        //                                        .Select(lang => new SelectListItem { Value = lang.Code, Text = lang.Description });
        //    return htmlHelper.DropDownListFor(expression, selectList, false);
        //}
        //public static MvcHtmlString CulturesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showBlankOption = false, string blankOptionText = "Seçiniz")
        //{
        //    var controller = htmlHelper.ViewContext.Controller as BaseController;
        //    var selectList = controller.Client.Services.API.Localization.GetCultures()
        //                                        .Select(culture => new SelectListItem { Value = culture.ID.ToString(), Text = culture.Name });
        //    return htmlHelper.DropDownListFor(expression, selectList, false);
        //}
        //public static MvcHtmlString ParameterGroupsFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool showBlankOption = false, string blankOptionText = "Seçiniz")
        //{
        //    var controller = htmlHelper.ViewContext.Controller as BaseController;

        //    var selectList = controller.Client.Services.API.Configuration.GetParameterGroups()
        //                                         .Select(group => new SelectListItem { Value = group.Name, Text = group.Name });

        //    return htmlHelper.DropDownListFor(expression, selectList, false);
        //}
        //public static MvcHtmlString UriTypeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        //{
        //    return htmlHelper.EnumDropDownListFor<TModel, TProperty, UriType>(expression);
        //}
        //public static MvcHtmlString AnswerTypeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        //{
        //    return htmlHelper.EnumDropDownListFor<TModel, TProperty, AnswerType>(expression);
        //}
        //public static MvcHtmlString ParameterTypeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        //{
        //    return htmlHelper.EnumDropDownListFor<TModel, TProperty, ParameterType>(expression);
        //}
        //public static MvcHtmlString MessageTypeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        //{
        //    return htmlHelper.EnumDropDownListFor<TModel, TProperty, MessageType>(expression);
        //}
        public static MvcHtmlString StatusFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression)
        {
            return htmlHelper.SwitchFor(expression, "success", "default", "Aktif", "Pasif", "I");
        }


        //public static MvcHtmlString ComparisonTypeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        //{
        //    return htmlHelper.EnumDropDownListFor<TModel, TProperty, ComparisonType>(expression);
        //}
        public static MvcHtmlString ActionButtonsFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            bool allowReturnList = true, bool allowDelete = false, string listUrl = "", string deleteUrl = "", string saveButtonText = "Kaydet")
        {
            var type = typeof(TModel);
            var model = htmlHelper.ViewData.Model as EntityModel;
            allowDelete = allowDelete && !model.IsNew;
            var entityName = type.Name.Replace("Model", "");
            var builder = new StringBuilder();
            builder.Append("<div class='form-actions col-md-10'>");
            builder.Append(htmlHelper.Hidden("IsNew", model.IsNew));
            builder.Append("<button type='submit' name='action' value='save' id='save-")
                .Append(entityName.ToLowerInvariant())
                .Append("' class='btn-press gray pull-right mrn' title='Kaydet'>");
            builder.Append("<span>" + saveButtonText + "&nbsp;<i class='fa fa-save'></i></span>");
            builder.Append("</button>");
            if (allowDelete)
            {
                builder.Append("<button type='submit' name='action' value='delete' id='delete-")
                    .Append(entityName.ToLowerInvariant())
                    .Append("' class='btn-press yellow pull-right mls' title='Sil'>");
                builder.Append("<span>Sil&nbsp;<i class='fa fa-trash-o'></i></span>");
                builder.Append("</button>");
            }

            if (allowReturnList)
            {
                if (string.IsNullOrEmpty(listUrl))
                    listUrl = type.FullName.Replace("Dalowe.View.Web.Framework.Models", "").Replace(".", "/")
                        .Replace(type.Name, entityName + "List");
                builder.Append("<a class='btn-press yellow pull-right' title='Listeye geri dön' href='").Append(listUrl)
                    .Append("'>");
                builder.Append("<span><i class='fa fa-chevron-left'></i>&nbsp;Geri</span></a>");
            }

            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString CheckBoxFieldFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression, object htmlAttributes = null)
        {
            var type = typeof(TModel);
            var property = type.GetProperty(((MemberExpression)expression.Body).Member.Name);
            if (Attribute.IsDefined(property, typeof(SwitchAttribute)))
            {
                var switchAttribute = property.GetCustomAttribute<SwitchAttribute>();
                return htmlHelper.SwitchFor(expression, switchAttribute);
            }

            var defaultHtmlAttributes = new { @class = "form-control validate" };
            var attributes = htmlHelper.MergeHtmlAttributes(htmlAttributes, defaultHtmlAttributes);
            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div class='col-md-6'>");
            builder.Append(htmlHelper.CheckBoxFor(expression, new RouteValueDictionary(attributes)));
            builder.Append(htmlHelper.ValidationMessageFor(expression));
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString SliderFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div class='col-md-6'>");
            builder.Append(htmlHelper.HiddenFor(expression));
            builder.Append("<label id='slider-").Append(metaData.PropertyName.ToSlug())
                .Append("-value' class='text-primary' style='font-weight:bold;'></label>");
            builder.Append("<div id='slider-").Append(metaData.PropertyName.ToSlug()).Append("' class='slider'></div>");
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString PasswordFieldFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var defaultHtmlAttributes = new { @class = "form-control validate", autocomplete = "off" };
            var attributes = htmlHelper.MergeHtmlAttributes(htmlAttributes, defaultHtmlAttributes);

            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div class='col-md-6'>");
            builder.Append(htmlHelper.PasswordFor(expression, new RouteValueDictionary(attributes)));
            builder.Append(htmlHelper.ValidationMessageFor(expression, "",
                new RouteValueDictionary(new { @class = "help-block" })));
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString TextFieldFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var attributes = new RouteValueDictionary(new { @class = "form-control validate" });
            if (htmlHelper.ViewData.Model != null)
            {
                var type = typeof(TModel);
                var expressionMember = ((MemberExpression)expression.Body).Member;
                PropertyInfo property = null;
                type = expressionMember.DeclaringType;
                if (expressionMember.DeclaringType.Name == "EntityModel" ||
                    expressionMember.DeclaringType.BaseType.Name == "EntityModel" ||
                    expressionMember.DeclaringType.Name == "AuditModel" ||
                    expressionMember.DeclaringType.BaseType.Name == "AuditModel" ||
                    expressionMember.DeclaringType.Name == "LocalPasswordModel" ||
                    expressionMember.DeclaringType.BaseType.Name == "LocalPasswordModel")
                {
                    property = type.GetProperty(expressionMember.Name);
                }
                else
                {
                    var declaringTypeExpression = ((MemberExpression)expression.Body).Expression;
                    property = type.GetProperty(((MemberExpression)declaringTypeExpression).Member.Name).PropertyType
                        .GetProperty(expressionMember.Name);
                }

                if (Attribute.IsDefined(property, typeof(ColumnAttribute)))
                {
                    var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                    if (columnAttribute.Identity || columnAttribute.HashKey || columnAttribute.RangeKey)
                    {
                        var model = htmlHelper.ViewData.Model as EntityModel;
                        if (!model.IsNew)
                            attributes =
                                attributes.MergeAttributes(new RouteValueDictionary(new { @readonly = "readonly" }));
                    }
                }
            }

            if (htmlAttributes != null)
                attributes = attributes.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div class='col-md-9'>");
            builder.Append(htmlHelper.TextBoxFor(expression, attributes));
            builder.Append(htmlHelper.ValidationMessageFor(expression, "",
                new RouteValueDictionary(new { @class = "help-block" })));
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString RichTextFieldFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var attributes = new RouteValueDictionary(new { @class = "form-control validate" });
            if (htmlHelper.ViewData.Model != null)
            {
                var type = typeof(TModel);
                var property = type.GetProperty(((MemberExpression)expression.Body).Member.Name);
                if (Attribute.IsDefined(property, typeof(ColumnAttribute)))
                {
                    var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                    if (columnAttribute.Identity || columnAttribute.HashKey || columnAttribute.RangeKey)
                    {
                        var model = htmlHelper.ViewData.Model as EntityModel;
                        if (!model.IsNew)
                            attributes =
                                attributes.MergeAttributes(new RouteValueDictionary(new { @readonly = "readonly" }));
                    }
                }
            }

            if (htmlAttributes != null)
                attributes = attributes.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div class='col-md-9'>");
            builder.Append(htmlHelper.TextAreaFor(expression, attributes));
            builder.Append(htmlHelper.ValidationMessageFor(expression, "",
                new RouteValueDictionary(new { @class = "help-block" })));
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString DateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div class='col-md-6'>");
            builder.Append("<div class='input-group datetimepicker date'>");
            builder.Append(htmlHelper.TextBoxFor(expression,
                new RouteValueDictionary(new { @class = "form-control validate", type = "datetime" })));
            builder.Append("<span class='input-group-addon'><i class='fa fa-calendar'></i></span></div>");
            builder.Append(htmlHelper.ValidationMessageFor(expression));
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString VideoFor<TModel>(this HtmlHelper<TModel> htmlHelper, string filePath,
            object htmlAttributes = null)
        {
            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append("<div class='col-md-6'>");
            var videoBuilder = new TagBuilder("video");
            if (htmlAttributes != null)
            {
                var routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                videoBuilder.MergeAttributes(routeValueDictionary);
            }

            var srcBuilder = new TagBuilder("source");
            srcBuilder.MergeAttribute("type", MimeMapping.GetMimeMapping(filePath.Split('/').Last()));
            srcBuilder.MergeAttribute("src", filePath);
            videoBuilder.InnerHtml = srcBuilder.ToString(TagRenderMode.SelfClosing);
            builder.Append(videoBuilder.ToString(TagRenderMode.Normal));
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString DateRangeFieldFor<TModel>(this HtmlHelper<TModel> htmlHelper, string fieldLabel,
            Expression<Func<TModel, DateTime>> startDateExpression,
            Expression<Func<TModel, DateTime>> endDateExpression)
        {
            DateTime startDate = DateTime.Now, endDate = DateTime.Now;
            if (htmlHelper.ViewData.Model != null)
            {
                startDate = Convert.ToDateTime(startDateExpression.Compile().Invoke(htmlHelper.ViewData.Model));
                endDate = Convert.ToDateTime(endDateExpression.Compile().Invoke(htmlHelper.ViewData.Model));
            }

            if (startDate.Date <= DateTime.MinValue) startDate = DateTime.Now;
            if (endDate.Date <= DateTime.MinValue) endDate = DateTime.Now;

            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append("<label class='col-md-4 control-label'>").Append(fieldLabel).Append("</label>");
            builder.Append("<div class='col-md-6'>");
            builder.Append("<div class='btn daterangebar col-md-12'>");
            builder.Append(htmlHelper.HiddenFor(startDateExpression,
                new RouteValueDictionary(new { @class = "startdate" })));
            builder.Append(htmlHelper.HiddenFor(endDateExpression, new RouteValueDictionary(new { @class = "enddate" })));
            builder.Append("<i class='fa fa-calendar'></i>&nbsp;");
            builder.Append("<span>");
            builder.Append(startDate.ToString("dd MMM yyyy") + " - " + endDate.ToString("dd MMM yyyy"));
            builder.Append("</span>");
            builder.Append("&nbsp;&nbsp;<i class='fa fa-angle-down'></i>");
            builder.Append("</div></div></div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString IconUploadFieldFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            Expression<Func<TModel, HttpPostedFileBase>> uploadFileExpression = null, bool circleImage = false,
            bool borderedImage = false)
        {
            var imagePath = string.Empty;
            if (htmlHelper.ViewData.Model != null)
                imagePath = htmlHelper.GetImageUrl(
                    Convert.ToString(expression.Compile().Invoke(htmlHelper.ViewData.Model)));

            var uploadedFile = ((MemberExpression)expression.Body).Member.Name.Replace("FilePath", "")
                .Replace("Path", "").Replace("Url", "").ToLowerInvariant();
            var previewImage = uploadedFile + "Preview";
            var builder = new StringBuilder();

            builder.Append("<div class='form-group text-center'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div role='presentation' class='col-md-1'>");
            builder.Append("<div class='files col-md-12'>");
            builder.Append("<img id='").Append(previewImage).Append("' alt='").Append(imagePath).Append("' src='")
                .Append(imagePath).Append("' class='img-responsive").Append(circleImage ? " img-circle" : string.Empty)
                .Append(borderedImage ? " img-bordered" : string.Empty)
                .Append("' style='margin:0 auto;min-width:48px;min-height:48px;'/>");
            builder.Append("</div>");
            builder.Append(htmlHelper.HiddenFor(expression));
            builder.Append("<span class='fileinput-button'><i class='fa fa-upload'></i>&nbsp;");
            builder.Append("<span>Yükle</span>");
            if (uploadFileExpression != null)
                builder.Append(htmlHelper.TextBoxFor(uploadFileExpression,
                    new { type = "file", @class = "with-preview", data_preview = previewImage }));
            else
                builder.Append("<input type='file' name='").Append(uploadedFile).Append("' id='").Append(uploadedFile)
                    .Append("' class='with-preview' data-preview='").Append(previewImage).Append("' />");
            builder.Append("</span></div></div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString ImageUploadFieldFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            Expression<Func<TModel, HttpPostedFileBase>> uploadFileExpression = null, bool circleImage = false,
            bool borderedImage = false)
        {
            var imagePath = string.Empty;
            if (htmlHelper.ViewData.Model != null)
                imagePath = htmlHelper.GetImageUrl(
                    Convert.ToString(expression.Compile().Invoke(htmlHelper.ViewData.Model)));

            var uploadedFile = ((MemberExpression)expression.Body).Member.Name.Replace("FilePath", "")
                .Replace("Path", "").Replace("Url", "").ToLowerInvariant();
            var previewImage = uploadedFile + "Preview";
            var builder = new StringBuilder();

            builder.Append("<div class='form-group text-center'>");
            builder.Append("<div class='col-md-12 text-center mbl'>");
            builder.Append(htmlHelper.LabelFor(expression, new RouteValueDictionary(new { @class = "control-label" })));
            builder.Append("</div>");
            builder.Append("<div role='presentation'>");
            builder.Append("<div class='files col-md-12'>");
            builder.Append("<img id='").Append(previewImage).Append("' alt='").Append(imagePath).Append("' src='")
                .Append(imagePath).Append("' class='img-responsive fhs")
                .Append(circleImage ? " img-circle" : string.Empty)
                .Append(borderedImage ? " img-bordered" : string.Empty).Append("' style='margin:0 auto;'/>");
            builder.Append("</div></div>");
            builder.Append("<div class='col-md-12 text-center mtxxl'>");
            builder.Append(htmlHelper.HiddenFor(expression));
            builder.Append(
                "<span class='btn btn-success fileinput-button'><i class='glyphicon glyphicon-upload'></i>&nbsp;");
            builder.Append("<span>Yükle</span>");
            if (uploadFileExpression != null)
                builder.Append(htmlHelper.TextBoxFor(uploadFileExpression,
                    new { type = "file", @class = "with-preview", data_preview = previewImage }));
            else
                builder.Append("<input type='file' name='").Append(uploadedFile).Append("' id='").Append(uploadedFile)
                    .Append("' class='with-preview' data-preview='").Append(previewImage).Append("' />");
            builder.Append("</span></div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString FileUploadFieldFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            Expression<Func<TModel, HttpPostedFileBase>> uploadFileExpression = null)
        {
            var filePath = string.Empty;
            if (htmlHelper.ViewData.Model != null)
                filePath = Convert.ToString(expression.Compile().Invoke(htmlHelper.ViewData.Model));
            filePath = string.IsNullOrEmpty(filePath) ? "No-File" : filePath;

            var uploadedFile = ((MemberExpression)expression.Body).Member.Name.Replace("Path", "").Replace("Url", "")
                .ToLowerInvariant();
            var builder = new StringBuilder();

            builder.Append("<div class='form-group text-center'>");
            builder.Append("<div class='col-md-12 text-center mbl'>");
            builder.Append(htmlHelper.LabelFor(expression, new RouteValueDictionary(new { @class = "control-label" })));
            builder.Append("</div>");
            builder.Append("<div class='files col-md-12'>");
            builder.Append("<span class='filename col-md-12'>").Append(filePath).Append("</span>");
            builder.Append("</div>");
            builder.Append("<div class='col-md-12 text-center mtxxl'>");
            builder.Append(htmlHelper.HiddenFor(expression));
            builder.Append(
                "<span class='btn btn-success fileinput-button'><i class='glyphicon glyphicon-upload'></i>&nbsp;");
            builder.Append("<span>Yükle</span>");
            if (uploadFileExpression != null)
                builder.Append(htmlHelper.TextBoxFor(uploadFileExpression, new { type = "file" }));
            else
                builder.Append("<input type='file' class='no-preview' name='").Append(uploadedFile).Append("' id='")
                    .Append(uploadedFile).Append("' />");
            builder.Append("</span></div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString VideoUploadFieldFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            Expression<Func<TModel, HttpPostedFileBase>> uploadFileExpression = null)
        {
            var videoPath = string.Empty;
            if (htmlHelper.ViewData.Model != null)
                videoPath = htmlHelper.GetImageUrl(
                    Convert.ToString(expression.Compile().Invoke(htmlHelper.ViewData.Model)));

            var uploadedFile = ((MemberExpression)expression.Body).Member.Name.Replace("FilePath", "")
                .Replace("Path", "").Replace("Url", "").ToLowerInvariant();
            var previewImage = uploadedFile + "Preview";
            var builder = new StringBuilder();

            builder.Append("<div class='form-group text-center'>");
            builder.Append("<div class='col-md-12 text-center mbl'>");
            builder.Append(htmlHelper.LabelFor(expression, new RouteValueDictionary(new { @class = "control-label" })));
            builder.Append("</div>");
            builder.Append("<div role='presentation'>");
            builder.Append("<div class='files col-md-12'>");
            builder.Append("<img id='").Append(previewImage).Append("' class='img-responsive fhs")
                .Append("' style='margin:0 auto;'/>");
            builder.Append("</div></div>");
            builder.Append("<div class='col-md-12 text-center mtxxl'>");
            builder.Append(htmlHelper.HiddenFor(expression));
            builder.Append(
                "<span class='btn btn-success fileinput-button'><i class='glyphicon glyphicon-upload'></i>&nbsp;");
            builder.Append("<span>Yükle</span>");
            if (uploadFileExpression != null)
                builder.Append(htmlHelper.TextBoxFor(uploadFileExpression,
                    new { type = "file", @class = "with-preview", data_preview = previewImage }));
            else
                builder.Append("<input type='file' name='").Append(uploadedFile).Append("' id='").Append(uploadedFile)
                    .Append("' class='with-preview' data-preview='").Append(previewImage).Append("' />");
            builder.Append("</span></div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString ColorPickerFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            var color = string.Empty;
            if (htmlHelper.ViewData.Model != null)
                color = expression.Compile().Invoke(htmlHelper.ViewData.Model) as string;

            if (!string.IsNullOrWhiteSpace(color) && !color.StartsWith("#"))
                color = "#" + color;

            var builder = new StringBuilder();
            builder.Append("<div class='form-group'>");
            builder.Append(htmlHelper.ControlLabelFor(expression));
            builder.Append("<div class='col-md-6'>");
            builder.Append("<div data-color='").Append(color)
                .Append("' data-color-format='rgba' class='input-group colorpicker-component'>");
            builder.Append(htmlHelper.TextBoxFor(expression,
                new RouteValueDictionary(new
                {
                    @class = "form-control validate",
                    pattern = "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
                    placeholder = "Örn; #F0F0F0"
                })));
            builder.Append("<span class='input-group-btn input-group-btn-grey'>");
            builder.Append("<button type='button' class='btn'>");
            if (string.IsNullOrWhiteSpace(color)) color = "#5cb85c";
            builder.Append("<i style='color:").Append(color).Append(";' class='fa fa-certificate'></i>");
            builder.Append("</button></span></div>");
            builder.Append(htmlHelper.ValidationMessageFor(expression));
            builder.Append("</div>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString ControlLabelFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var attributes = new RouteValueDictionary(new { @class = "control-label" });

            if (htmlAttributes != null)
                attributes = attributes.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            var type = typeof(TModel);
            var expressionMember = ((MemberExpression)expression.Body).Member;
            type = expressionMember.DeclaringType;
            PropertyInfo property = null;
            if (expressionMember.DeclaringType.Name == "EntityModel" ||
                expressionMember.DeclaringType.BaseType.Name == "EntityModel" ||
                expressionMember.DeclaringType.Name == "AuditModel" ||
                expressionMember.DeclaringType.BaseType.Name == "AuditModel" ||
                expressionMember.DeclaringType.Name == "LocalPasswordModel" ||
                expressionMember.DeclaringType.BaseType.Name == "LocalPasswordModel")
            {
                property = type.GetProperty(expressionMember.Name);
            }
            else
            {
                var declaringTypeExpression = ((MemberExpression)expression.Body).Expression;
                property = declaringTypeExpression.Type.GetProperty(expressionMember.Name);
                //property = type.GetProperty(((MemberExpression) declaringTypeExpression).Member.Name).PropertyType
                //    .GetProperty(expressionMember.Name);
            }

            var builder = new StringBuilder();
            builder.Append("<div class='col-md-3 text-right-o'>");
            if (property != null && Attribute.IsDefined(property, typeof(RequiredAttribute)))
                builder.Append("<span class='require'>* </span>");
            builder.Append(htmlHelper.LabelFor(expression, attributes));
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString TableFor<TModel, TEntity>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, IEnumerable<TEntity>>> expression, int page = 1, int pageSize = 25, bool allowNew = true)
            where TEntity : class
        {
            var type = typeof(TEntity);
            var tableInfo = type.GetCustomAttribute<TableAttribute>();
            if (tableInfo == null) return null;

            List<TEntity> items = null;
            if (htmlHelper.ViewData.Model != null)
                items = expression.Compile().Invoke(htmlHelper.ViewData.Model).ToList();
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var tableName = string.IsNullOrEmpty(tableInfo.MemberName)
                ? type.Name.ToLowerInvariant().Replace("model", "-list")
                : tableInfo.MemberName;
            var typeName = type.Name.Replace("Model", "");
            var columns = type.GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(ColumnAttribute)))
                .OrderBy(prop => prop.GetCustomAttribute<ColumnAttribute>().Index).ToList();
            var identityProperty = columns.FirstOrDefault(c => c.GetCustomAttribute<ColumnAttribute>().Identity);
            var counter = 0;
            var builder = new StringBuilder();

            builder.Append("<div class='portlet'>");
            builder.Append("<div class='portlet-header'>");
            if (!string.IsNullOrEmpty(tableInfo.Title))
                builder.Append("<div class='caption'>").Append(tableInfo.Title).Append("</div>");
            builder.Append("<div class='tools'>");
            if (tableInfo.AllowNew && allowNew)
            {
                builder.Append("<a id='add-").Append(typeName.ToLowerInvariant())
                    .Append("' class='btn btn-primary btn-small' href='");
                if (tableInfo.AllowEdit && !string.IsNullOrEmpty(tableInfo.EditUrl))
                    builder.Append(tableInfo.EditUrl).Append("/");
                else
                    builder.Append("javascript:;");
                builder.Append("' title='Yeni'>");
                builder.Append("<span>Yeni&nbsp;<i class='fa fa-plus'></i></span>");
                builder.Append("</a>");
            }

            if (tableInfo.AllowExpand)
                builder.Append("<i class='fa fa-chevron-up'></i>");
            if (tableInfo.AllowRefresh)
                builder.Append("<i class='fa fa-refresh'></i>");
            builder.Append("</div>");
            builder.Append("</div>");
            builder.Append("<div class='portlet-body'>");
            builder.Append("<div class='table-responsive mtl'>");
            builder.Append("<table id='");
            builder.Append(tableName);
            builder.Append("' class='table table-striped table-bordered table-hover order-column cell-border'")
                .Append(" data-editurl='").Append(tableInfo.EditUrl).Append("' data-type='").Append(typeName)
                .Append("'>");
            var accessor = new Accessor();
            ColumnAttribute columnInfo;
            builder.Append("<thead>");
            builder.Append("<tr>");
            foreach (var column in columns)
            {
                columnInfo = column.GetCustomAttribute<ColumnAttribute>();
                if (column.PropertyType == typeof(Dictionary<string, string>))
                {
                    var dictionary = (Dictionary<string, string>)column.GetValue(items[0]);
                    foreach (var dicItem in dictionary)
                    {
                        builder.Append("<th>");
                        builder.Append(dicItem.Key);
                        builder.Append("</th>");
                    }
                }
                else
                {
                    builder.Append("<th>");
                    builder.Append(columnInfo.Title);
                    builder.Append("</th>");
                }
            }

            if (tableInfo.ShowRowProcess)
                builder.Append("<th>İşlemler</th>");

            builder.Append("</tr>");
            builder.Append("</thead>");
            builder.Append("<tbody>");
            if (items != null && items.Count > 0)
                foreach (var item in items)
                {
                    accessor.Item = item;
                    builder.Append("<tr data-type='").Append(typeName).Append("'");
                    if (identityProperty != null)
                    {
                        accessor.MemberName = identityProperty.Name;
                        builder.Append(" data-id='").Append(accessor.Value).Append("'");
                    }

                    builder.Append(">");
                    for (var i = 0; i < columns.Count; i++)
                    {
                        var column = columns[i];

                        if (column.PropertyType == typeof(Dictionary<string, string>))
                        {
                            var dictionary = (Dictionary<string, string>)column.GetValue(item);
                            foreach (var dicItem in dictionary)
                            {
                                builder.Append($"<td>{dicItem.Value}</td>");
                            }
                        }
                        else
                        {
                            AddColumnValue(builder, accessor, htmlHelper, columns[i]);
                        }
                    }

                    if (tableInfo.ShowRowProcess)
                    {
                        accessor.MemberName = "History";
                        builder.Append("<td>");
                        builder.Append(htmlHelper.EditButtonGroup((AuditModel)accessor.Value, tableInfo.AllowEdit));
                        builder.Append("</td>");
                    }
                    builder.Append("</tr>");
                    counter++;
                }

            builder.Append("</tbody></table>");
            builder.Append("</div>");
            builder.Append("</div>");
            builder.Append("</div>");
            return new MvcHtmlString(builder.ToString());
        }

        private static void AddColumnValue(StringBuilder builder, Accessor accessor, HtmlHelper htmlHelper, PropertyInfo column)
        {
            accessor.MemberName = column.Name;
            var columnInfo = column.GetCustomAttribute<ColumnAttribute>();

            builder.Append("<td>");
            if (Attribute.IsDefined(column, typeof(SwitchAttribute)))
            {
                builder.Append(
                    "<div data-on='success' data-off='default' class='make-switch switch-small auto-update' data-on-label='Aktif' data-off-label='Pasif' data-text-label='I'>");
                builder.Append("<input type='checkbox'");
                if (Convert.ToBoolean(accessor.Value))
                    builder.Append(" checked='checked'");
                builder.Append("/></div>");
            }
            else if (columnInfo.PreviewAs == PreviewType.Image)
            {
                builder.Append("<img width='24' src='")
                    .Append(htmlHelper.GetImageUrl((string)accessor.Value, false)).Append("'/>");
            }
            else if (columnInfo.PreviewAs == PreviewType.Icon)
            {
                builder.Append(accessor.Value.ToString().ToEnum<AwesomeIcons>().Render());
            }
            else if (column.PropertyType.IsEnum)
            {
                builder.Append(accessor.Value?.GetType()?.GetField(accessor.Value.ToString())?.GetCustomAttribute<StringValueAttribute>()?.StringValue ?? accessor.Value);
            }
            else
            {
                builder.Append(accessor.Value);
            }

            builder.Append("</td>");
        }

        internal static MvcHtmlString TableCustomizeScript()
        {
            var script = new StringBuilder();
            script.Append("<script type='text/javascript'>$(document).ready(function () {");
            script.Append("var table = $('#").Append("addresstype-list").Append("').dataTable({");
            script.Append("'aoColumnDefs': [{ 'aTargets': [0], 'bVisible': false, 'bSearchable': false },");
            script.Append(
                "{ 'aTargets': [2], 'fnCreatedCell': function (td, data) { $(td).html(data == '1' ? 'Aktif' : 'Pasif'); } },");
            script.Append("{ 'aTargets': [3], 'bSearchable': false, 'bSortable': false, 'sWidth': '110px' }");
            script.Append("],");
            script.Append("'bAutoWidth': false,");
            script.Append("});");

            script.Append("var nEditing = null;");

            script.Append("$('#").Append("addresstype-list").Append("')");
            script.Append(
                @".on('click', 'a.edit', function (e) { e.preventDefault(); var nRow = $(this).parents('tr')[0]; if (nEditing !== null && nEditing != nRow) { rollbackRow(table, nEditing); editRow(table, nRow, false); nEditing = nRow; } else if (nEditing == nRow && $('i', this).hasClass('fa-save')) { saveRow(table, nEditing); nEditing = null; } else { editRow(table, nRow, false); nEditing = nRow; } });");
            script.Append("$('#").Append("addresstype-list").Append("')");
            script.Append(
                @".on('click', 'a.delete', function (e) { e.preventDefault(); $.Arwend.Dialogs.InformationBox('Uyarı', 'Silmek istediğinize emin misiniz ?'); var nRow = $(this).parents('tr')[0]; table.fnDeleteRow(nRow); });");
            script.Append("$('#").Append("addresstype-list").Append("')");
            script.Append(
                @".on('click', 'a.cancel', function (e) { e.preventDefault(); if ($(this).attr('data-mode') == 'new') { var nRow = $(this).parents('tr')[0]; table.fnDeleteRow(nRow); } else { rollbackRow(table, nEditing); nEditing = null; } });");
            script.Append(@"function editRow(table, nRow, isNewRow) {
                var aData = table.fnGetData(nRow);
                var rTds = $('>td', nRow);
                for (var i = 1; i < rTds.length; i++) {
                    rTds[i - 1].innerHTML = '<input type='text' class='form-control input-large' value=\'' + aData[i] + '\'>';
                }
                $.BackOfficer.GetActionsColumn(nRow, 'Edit', 2, isNewRow);
            }

            function saveRow(table, nRow) {
                var rInputs = $('input', nRow);
                for (var i = 0; i < rInputs.length; i++) {
                    table.fnUpdate(rInputs[i].value, nRow, i + 1, false);
                }
                $.BackOfficer.GetActionsColumn(nRow, 'Read', 2);
                var aData = table.fnGetData(nRow);
                $.Arwend.Ajax.Parameters = {
                    Action: 'editaddresstype',
                    AddressTypeID: aData[0],
                    Name: aData[1],
                    Status: aData[2]
                }
                $.Arwend.Ajax.GetResponse(function () { table.fnDraw(); });
            }
            function rollbackRow(table, nRow) {
                var aData = table.fnGetData(nRow);
                var rTds = $('>td', nRow);
                for (var i = 0; i < rTds.length; i++) {
                    table.fnUpdate(aData[i + 1], nRow, i + 1, false);
                }
                table.fnDraw();
            }
            $('#new').click(function (e) {
                e.preventDefault();
                if ($('a[data-mode='new']', table).length == 0) {
                    var aiNew = table.fnAddData([0, '', 1, '']);
                    var nRow = table.fnGetNodes(aiNew[0]);
                    if (nEditing !== null)
                        rollbackRow(table, nEditing);

                    nEditing = nRow;
                    editRow(table, nRow, true);
                }
            });
        });
    </script>");
            return script.ToMvcHtml();
        }

        public static MvcHtmlString EditButtonGroup(this HtmlHelper htmlHelper, AuditModel history, bool allowEdit)
        {
            var builder = new StringBuilder();
            builder.Append("<div class='btn-group'>");
            builder.Append("<button class='btn btn-primary btn-sm' type='button'>İşlemler</button>");
            builder.Append(
                "<button class='btn btn-primary btn-sm dropdown-toggle' data-toggle='dropdown' type='button'>");
            builder.Append("<span class='caret'></span><span class='sr-only'>Toggle Dropdown</span>");
            builder.Append("</button>");
            builder.Append("<ul class='dropdown-menu pull-right' role='menu'>");
            if (allowEdit)
                builder.Append(
                    "<li><a href='javascript:;' class='edit'><i class='fa fa-edit'></i>&nbsp;Düzenle</a></li>");
            builder.Append("<li><a href='javascript:;' class='delete'><i class='fa fa-trash-o'></i>&nbsp;Sil</a></li>");
            if (history != null)
            {
                builder.Append("<li class='divider'></li>");
                builder.Append(
                    "<li><a data-width='600px' data-container='body' data-toggle='popover' data-placement='left' data-html='true'");
                builder.Append(" data-content='<b>Güncelleyen : </b>");
                builder.Append(string.IsNullOrEmpty(history.UserModifiedName)
                    ? history.UserModifiedID.ToString()
                    : history.UserModifiedName);
                builder.Append("<br/><b>Güncel. Tarihi : </b>");
                builder.Append(history.DateModified.ToTrString("dd MMM yyyy, ddd HH:mm"));
                builder.Append("<br/><b>Oluşturan : </b>");
                builder.Append(string.IsNullOrEmpty(history.UserCreatedName)
                    ? history.UserCreatedID.ToString()
                    : history.UserCreatedName);
                builder.Append("<br/><b>Oluşt. Tarihi : </b>");
                builder.Append(history.DateCreated.ToTrString("dd MMM yyyy, ddd HH:mm"));
                builder.Append("' class='text-center' href='javascript:;'>Güncelleme Bilgisi</a></li>");
            }

            builder.Append("</ul>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString Breadcrumbs(this HtmlHelper htmlHelper)
        {
            var breadcrumbs = new[] { new BreadcrumbsItemModel { Text = htmlHelper.ViewBag.Title } };
            return DrawBreadcrumbs(htmlHelper, breadcrumbs);
        }

        public static MvcHtmlString BreadcrumbsFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, IEnumerable<BreadcrumbsItemModel>>> expression)
        {
            var breadcrumbs = expression.Compile().Invoke(htmlHelper.ViewData.Model) as List<BreadcrumbsItemModel>;
            if (!breadcrumbs.Any(b => b.Text == htmlHelper.ViewBag.Title))
                breadcrumbs.Add(new BreadcrumbsItemModel { Text = htmlHelper.ViewBag.Title });
            breadcrumbs.Last().IsActive = false;
            return DrawBreadcrumbs(htmlHelper, breadcrumbs);
        }

        internal static MvcHtmlString DrawBreadcrumbs(HtmlHelper htmlHelper,
            IEnumerable<BreadcrumbsItemModel> breadcrumbs)
        {
            var currentItem = breadcrumbs.Last();
            var builder = new StringBuilder();
            builder.Append("<div class='page-header-breadcrumb'>");
            builder.Append("<div class='page-heading hidden-xs'>");
            builder.Append("<h1 class='page-title'>");
            builder.Append(currentItem.Text);
            builder.Append("</h1>");
            builder.Append("</div>");
            builder.Append("<ol class='breadcrumb page-breadcrumb'>");
            builder.Append(
                "<li><i class='fa fa-dashboard'></i>&nbsp;<a href='/'>Genel Bakış</a>&nbsp;&nbsp;<i class='fa fa-angle-right'></i>&nbsp;&nbsp;</li>");
            foreach (var item in breadcrumbs)
            {
                builder.Append("<li").Append(item == currentItem ? " class='active'>" : ">");
                if (!item.IsActive)
                {
                    builder.Append(item.Text);
                }
                else
                {
                    builder.Append("<a href='");
                    builder.Append(htmlHelper.GenerateUrl(item.ActionName, item.ControllerName,
                        HtmlHelper.AnonymousObjectToHtmlAttributes(item.RouteValues)));
                    builder.Append("' title='");
                    builder.Append(item.Title);
                    builder.Append("'></a>");
                }

                if (item != currentItem)
                    builder.Append("&nbsp;&nbsp;<i class='fa fa-angle-right'></i>&nbsp;&nbsp;");
                builder.Append("</li>");
            }

            builder.Append("</ol>");
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString HistoryPanelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var builder = new StringBuilder();
            var model = expression.Compile().Invoke(htmlHelper.ViewData.Model) as AuditModel;
            builder.Append("<div id='center_column' class='center_column col-xs-12 col-sm-12'>");
            builder.Append("<h4 class='page-subheading top-indent'>Güncelleme Geçmişi</h4>");
            builder.Append(htmlHelper.TextFieldFor(m => model.UserCreatedName, new { disabled = "true" }));
            builder.Append(htmlHelper.TextFieldFor(m => model.DateCreated, new { disabled = "true" }));
            builder.Append(htmlHelper.TextFieldFor(m => model.UserModifiedName, new { disabled = "true" }));
            builder.Append(htmlHelper.TextFieldFor(m => model.DateModified, new { disabled = "true" }));
            builder.Append("</div>");
            return builder.ToMvcHtml();
        }

        public static MvcHtmlString ModalFor(this HtmlHelper htmlHelper, string modalName)
        {
            return htmlHelper.Partial(string.Format("~/Views/Shared/Modals/_{0}.cshtml", modalName));
        }

        public static string GetImageUrl(this HtmlHelper htmlHelper, string filePath, bool useCdn = true)
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = "images/no-image.gif";
            return (useCdn && !string.IsNullOrEmpty(ConfigurationManager.CdnUrl)
                       ? ConfigurationManager.CdnUrl
                       : "/files/") + filePath;
        }
    }
}