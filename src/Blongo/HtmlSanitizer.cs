namespace Blongo
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class HtmlSanitizer
    {
        private static readonly Regex _aTag;
        private static readonly Regex _basicTags;
        private static readonly Regex _closingTag;
        private static readonly Regex _imgTag;
        private static readonly Regex _openingTag;
        private static readonly Regex _tag;
        private static readonly Regex _tagName;

        static HtmlSanitizer()
        {
            _openingTag = new Regex("<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _closingTag = new Regex(@"^<\/", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _tag = new Regex(@"<\/?\w+[^>]*(\s|$|>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _tagName = new Regex(@"<\/?(\w+).*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            // Tags that can be opened/closed & tags that stand alone
            _basicTags =
                new Regex(
                    @"^(<\/?(b|blockquote|code|del|dd|dl|dt|em|h1|h2|h3|i|kbd|li|ol|p|pre|s|sup|sub|strong|strike|ul)>|<(br|hr)\s?\/?>)$",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);

            // <a href="{url}" title?>...</a>
            _aTag =
                new Regex(
                    @"^(<a\shref=""((https?|ftp):\/\/|\/)[-A-Za-z0-9+&@#\/%?=~_|!:,.;\(\)]+""(\stitle=""[^""<>]+"")?\s?>|<\/a>)$",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);

            // <img src="{url}" width? height? alt? title?/>
            _imgTag =
                new Regex(
                    @"^(<img\ssrc=""(https?:\/\/|\/)[-A-Za-z0-9+&@#\/%?=~_|!:,.;\(\)]+""(\swidth=""\d{1,3}"")?(\sheight=""\d{1,3}"")?(\salt=""[^""<>]*"")?(\stitle=""[^""<>]*"")?\s?\/?>)$",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public HtmlSanitizer(string html)
        {
            Html = html;
            SanitizedHtml = SanitizeHtml(html);
        }

        public string Html { get; }

        public string SanitizedHtml { get; }

        private static string SanitizeHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return html;
            }

            var sanitizedHtml = _openingTag.Replace(html, delegate(Match match)
            {
                var tag = match.Value;

                return _basicTags.IsMatch(tag) || _aTag.IsMatch(tag) || _imgTag.IsMatch(tag) ? tag : "";
            });

            return BalanceTags(sanitizedHtml);
        }

        private static string BalanceTags(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return "";
            }

            // Convert everything to lower case; this makes our case insensitive comparisons easier
            var tags = _tag.Matches(html.ToLowerInvariant());

            // No HTML tags present? Nothing to do; exit now
            var tagCount = tags.Count;

            if (tagCount == 0)
            {
                return html;
            }

            var ignoredTags = "<p><img><br><li><hr>";
            var tagsPaired = new List<int>();
            var tagsToRemove = new List<int>();

            // Loop through matched tags in forward order
            for (var tagIndex = 0; tagIndex < tagCount; tagIndex++)
            {
                var tag = tags[tagIndex].Value;
                var tagName = _tagName.Replace(tag, "$1");

                // Skip any already paired tags and skip tags in our ignore list; assume they're self-closed
                if (tagsPaired.Contains(tagIndex) || ignoredTags.Contains($"<{tagName}>"))
                {
                    continue;
                }

                var pairedTagIndex = -1;

                if (!_closingTag.IsMatch(tag))
                {
                    // This is an opening tag search forwards (next tags), look for closing tags
                    for (var nextTagIndex = tagIndex + 1; nextTagIndex < tagCount; nextTagIndex++)
                    {
                        if (!tagsPaired.Contains(nextTagIndex) && tags[nextTagIndex].Value == $"</{tagName}>")
                        {
                            pairedTagIndex = nextTagIndex;
                            break;
                        }
                    }
                }

                if (pairedTagIndex == -1)
                {
                    tagsToRemove.Add(tagIndex);
                }
                else
                {
                    tagsPaired.Add(pairedTagIndex); // Mark paired
                }
            }

            if (!tagsToRemove.Any())
            {
                return html;
            }

            // Delete all orphaned tags from the string
            var removeTagIndex = 0;

            html = _tag.Replace(html, delegate(Match m)
            {
                var removeTag = tagsToRemove.Contains(removeTagIndex);

                var tag = removeTag ? "" : m.Value;
                removeTagIndex++;

                return tag;
            });

            return html;
        }
    }
}