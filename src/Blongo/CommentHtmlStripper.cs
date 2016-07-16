using System.Text.RegularExpressions;

namespace Blongo
{
    public class CommentHtmlStripper
    {
        public CommentHtmlStripper(string html)
        {
            Html = html;
            StrippedHtml = StripHtml(html);
        }

        private static string StripHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return html;
            }

            return Regex.Replace(html, @"<(?!a(?: href=""[^""]*""(?: title=""[^""]*"")?)?>|\/a>|\/?b>|\/?blockquote>|\/?code>|\/?del>|\/?dd>|\/?dl>|\/?dt>|\/?em>|\/?h1>|\/?h2>|\/?h3>|\/?i(?: class=""[^""]*"")?>|img(?: src=""[^""]*""(?: width=""\d{1,3}(?:%|ch|em|ex|in|cm|mm|pt|pc|rem|vh|vmin|mvmax|vw)?""(?: height=""\d{1,3}(?:%|ch|em|ex|in|cm|mm|pt|pc|rem|vh|vmin|mvmax|vw)?""(?: alt=""[^""]*""(?: title=""[^""]*"")?)?)?)?)? ?\/?>|\/?kbd>|\/?li>|\/?ol>|\/?p>|\/?pre(?: class=""[^""]*"")?>|\/?s>|\/?sup>|\/?sub>|\/?strong>|\/?strike>|\/?ul>|br ?\/?>|hr ?\/?>)[^>]*>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public string Html { get; }

        public string StrippedHtml { get; }
    }
}