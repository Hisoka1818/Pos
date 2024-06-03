using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pos.Web.Core.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlContent HttpMethod(this IHtmlHelper htmlHelper, string method)
        {
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("type", "hidden");
            tagBuilder.MergeAttribute("name", "_method");
            tagBuilder.MergeAttribute("value", method);
            return tagBuilder.RenderSelfClosingTag();
        }
    }
}
