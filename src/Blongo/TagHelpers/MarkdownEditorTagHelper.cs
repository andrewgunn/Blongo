using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.WebEncoders;
using System.Threading.Tasks;

namespace Blongo.TagHelpers
{
    [HtmlTargetElement("markdowneditor")]
    public class MarkdownEditorTagHelper : TagHelper
    {
        public MarkdownEditorTagHelper(IHtmlGenerator htmlGenerator, IHtmlEncoder htmlEncoder)
        {
            _htmlGenerator = htmlGenerator;
            _htmlEncoder = htmlEncoder;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder editor;

            if (For == null)
            {
                editor = new TagBuilder("textarea");
                editor.MergeAttribute("id", "wmd-input");
                editor.AddCssClass("wmd-input");
            }
            else {
                editor = _htmlGenerator.GenerateTextArea(ViewContext, For.ModelExplorer, For.Name, 0, 0, new { id = "wmd-input", @class = "wmd-input" });
            }

            foreach (var attribute in context.AllAttributes)
            {
                editor.MergeAttribute(attribute.Name, attribute.Value.ToString(), true);
            }

            var childContent = await output.GetChildContentAsync();
            editor.InnerHtml.AppendHtml(childContent.GetContent());

            if (editor != null)
            {
                output.SuppressOutput();

                output.Content.SetHtmlContent(editor);
            }
        }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private const string ForAttributeName = "asp-for";

        private readonly IHtmlEncoder _htmlEncoder;
        private readonly IHtmlGenerator _htmlGenerator;
    }
}
