using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Blongo.TagHelpers
{
    [HtmlTargetElement("markdownpreview")]
    public class MarkdownPreviewTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var preview = new TagBuilder("div");
            preview.MergeAttribute("id", "wmd-preview");
            preview.AddCssClass("wmd-preview");

            foreach (var attribute in context.AllAttributes)
            {
                preview.MergeAttribute(attribute.Name, attribute.Value.ToString(), true);
            }

            output.SuppressOutput();

            output.Content.SetHtmlContent(preview);
        }
    }
}
