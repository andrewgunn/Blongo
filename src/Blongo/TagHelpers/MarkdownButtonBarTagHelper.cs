using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Blongo.TagHelpers
{
    [HtmlTargetElement("markdownbuttonbar")]
    public class MarkdownButtonBarTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var buttonBar = new TagBuilder("div");
            buttonBar.MergeAttribute("id", "wmd-button-bar");
            buttonBar.AddCssClass("wmd-button-bar");

            foreach (var attribute in context.AllAttributes)
            {
                buttonBar.MergeAttribute(attribute.Name, attribute.Value.ToString(), true);
            }

            output.SuppressOutput();

            output.Content.SetHtmlContent(buttonBar);
        }
    }
}
