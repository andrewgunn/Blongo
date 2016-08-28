namespace Blongo.TagHelpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("img", Attributes = DefaultAttributeName)]
    [HtmlTargetElement("img", Attributes = EmailAddressAttributeName)]
    [HtmlTargetElement("img", Attributes = SizeAttributeName)]
    public class GravatarTagHelper : TagHelper
    {
        private const string DefaultAttributeName = "asp-gravatar-default";
        private const string EmailAddressAttributeName = "asp-gravatar-emailaddress";
        private const string SizeAttributeName = "asp-gravatar-size";

        [HtmlAttributeName(DefaultAttributeName)]
        public string Default { get; set; }

        [HtmlAttributeName(EmailAddressAttributeName)]
        public string EmailAddress { get; set; }

        [HtmlAttributeName(SizeAttributeName)]
        public string Size { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var md5Hash = GenerateMd5Hash(EmailAddress);
            var scheme = ViewContext.HttpContext.Request.Scheme;
            const string host = "gravatar.com";
            var port = 80;
            var path = $"/avatar/{md5Hash}";
            var uriBuilder = new UriBuilder(scheme, host, port, path);
            uriBuilder.Query = $"d={Default}&s={Size}";

            output.Attributes.SetAttribute("src", uriBuilder.ToString());
        }

        private string GenerateMd5Hash(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value.Trim());
            var hash = md5.ComputeHash(inputBytes);
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("X2"));
            }

            return stringBuilder.ToString().ToLowerInvariant();
        }
    }
}