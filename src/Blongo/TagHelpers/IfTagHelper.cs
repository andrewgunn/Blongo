using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Blongo.TagHelpers
{
    [HtmlTargetElement(Attributes = ConditionAttributeName)]
    public class IfTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Condition)
            {
                output.SuppressOutput();
            }
        }

        [HtmlAttributeName(ConditionAttributeName)]
        public bool Condition { get; set; }

        private const string ConditionAttributeName = "asp-if";
    }
}
