using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;

namespace Blongo.TagHelpers
{
    [HtmlTargetElement("meta", Attributes = ContentAttributeName)]
    public class MetaTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Content.StartsWith("/"))
            {
                output.Attributes.SetAttribute("content", Content);

                return;
            }

            var request = ViewContext.HttpContext.Request;
            var scheme = request.Scheme;
            var hostParts = request.Host.Value.Split(':');
            var host = hostParts[0];
            var port = 80;

            if (hostParts.Count() > 1)
            {
                int.TryParse(hostParts[1], out port);
            }


            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = scheme;
            uriBuilder.Host = host;
            uriBuilder.Port = port;

            var contentParts = Content.Split('?');
            uriBuilder.Path = contentParts[0];

            if (contentParts.Length > 1)
            {
                uriBuilder.Query = contentParts[1];
            }

            var absoluteUri = uriBuilder.Uri.AbsoluteUri.TrimEnd('/');

            output.Attributes.SetAttribute("content", absoluteUri);
        }

        [HtmlAttributeName(ContentAttributeName)    ]
        public string Content { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private const string ContentAttributeName = "content";
    }
}
