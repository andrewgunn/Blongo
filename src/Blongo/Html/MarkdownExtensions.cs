using MarkdownSharp;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blongo.Html
{
    public static class MarkdownExtensions
    {
        public static IHtmlContent Markdown(this IHtmlHelper htmlHelper, string text)
        {
            var markdownTransformer = new Markdown();
            var html = markdownTransformer.Transform(text);

            return new HtmlString(html);
        }
    }
}
