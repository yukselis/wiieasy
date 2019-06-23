using System.Web.Mvc;

namespace Arwend.Web.View.Mvc.Html
{
    public static class HtmlEditor
    {
        public static MvcHtmlString HtmlEditorFor(this HtmlHelper htmlHelper, string name)
        {
            TagBuilder builder = new TagBuilder("script");
            builder.MergeAttribute("type", "text/javascript");
            builder.InnerHtml = "KindEditor.ready(function(editor) {editor.create('#" + name + "',{langType : 'en', width:'100%'}); });";
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
    }
}
