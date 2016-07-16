using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Blongo.TagHelpers
{
    [HtmlTargetElement(Attributes = ConditionAttributeName)]
    public class VisibleTagHelper : TagHelper
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

        private const string ConditionAttributeName = "asp-visible";
    }
}
