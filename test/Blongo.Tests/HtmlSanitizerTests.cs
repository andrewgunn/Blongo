using FluentAssertions;
using NUnit.Framework;
using System.Collections;

namespace Blongo.Tests
{
    public class HtmlSanitizerTests
    {
        [TestCaseSource("HtmlSanitizerTestData")]
        public void HtmlSanitizer_SanitizedHtml(string input, string expected)
        {
            var htmlSanitizer = new HtmlSanitizer(input);
            var sanitizedHtml = htmlSanitizer.SanitizedHtml;

            sanitizedHtml.Should().Be(expected);
        }

        public static IEnumerable HtmlSanitizerTestData
        {
            get
            {
                yield return new object[] { "<a href=\"http://blog.andrewgunn.co.uk\">Andrew Gunn's Blog</a>", "<a href=\"http://blog.andrewgunn.co.uk\">Andrew Gunn's Blog</a>" };
                yield return new object[] { "<a href=\"http://blog.andrewgunn.co.uk\" title=\"Andrew Gunn's Blog\">Andrew Gunn's Blog</a>", "<a href=\"http://blog.andrewgunn.co.uk\" title=\"Andrew Gunn's Blog\">Andrew Gunn's Blog</a>" };
                yield return new object[] { "<b>...</b>", "<b>...</b>" };
                yield return new object[] { "<blockquote>...</blockquote>", "<blockquote>...</blockquote>" };
                yield return new object[] { "<br>", "<br>" };
                yield return new object[] { "<br/>", "<br/>" };
                yield return new object[] { "<br />", "<br />" };
                yield return new object[] { "</br>", "" };
                yield return new object[] { "<code>...</code>", "<code>...</code>" };
                yield return new object[] { "<code>...", "..." };
                yield return new object[] { "...</code>", "..." };
                yield return new object[] { "<del>...</del>", "<del>...</del>" };
                yield return new object[] { "<del>...", "..." };
                yield return new object[] { "...</del>", "..." };
                yield return new object[] { "<dd>...</dd>", "<dd>...</dd>" };
                yield return new object[] { "<dd>...", "..." };
                yield return new object[] { "...</dd>", "..." };
                yield return new object[] { "<dl>...</dl>", "<dl>...</dl>" };
                yield return new object[] { "<dl>...", "..." };
                yield return new object[] { "...</dl>", "..." };
                yield return new object[] { "<dt>...</dt>", "<dt>...</dt>" };
                yield return new object[] { "<dt>...", "..." };
                yield return new object[] { "...</dt>", "..." };
                yield return new object[] { "<em>...</em>", "<em>...</em>" };
                yield return new object[] { "<em>...", "..." };
                yield return new object[] { "...</em>", "..." };
                yield return new object[] { "<i>...</i>", "<i>...</i>" };
                yield return new object[] { "<i>...", "..." };
                yield return new object[] { "...</i>", "..." };
                yield return new object[] { "<h1>...</h1>", "<h1>...</h1>" };
                yield return new object[] { "<h1>...", "..." };
                yield return new object[] { "...</h1>", "..." };
                yield return new object[] { "<h2>...</h2>", "<h2>...</h2>" };
                yield return new object[] { "<h2>...", "..." };
                yield return new object[] { "...</h2>", "..." };
                yield return new object[] { "<h3>...</h3>", "<h3>...</h3>" };
                yield return new object[] { "<h3>...", "..." };
                yield return new object[] { "...</h3>", "..." };
                yield return new object[] { "<h4>...</h4>", "..." };
                yield return new object[] { "<h5>...</h5>", "..." };
                yield return new object[] { "<h6>...</h6>", "..." };
                yield return new object[] { "<hr>", "<hr>" };
                yield return new object[] { "<hr/>", "<hr/>" };
                yield return new object[] { "<hr />", "<hr />" };
                yield return new object[] { "<img src=\"http://blog.andrewgunn.co.uk/favicon-16x16.png\" />", "<img src=\"http://blog.andrewgunn.co.uk/favicon-16x16.png\" />" };
                yield return new object[] { "<img src=\"http://blog.andrewgunn.co.uk/favicon-16x16.png\" width=\"16\" height=\"16\" alt=\"Favicon\" />", "<img src=\"http://blog.andrewgunn.co.uk/favicon-16x16.png\" width=\"16\" height=\"16\" alt=\"Favicon\" />" };
                yield return new object[] { "<kbd>...</kbd>", "<kbd>...</kbd>" };
                yield return new object[] { "<kbd>...", "..." };
                yield return new object[] { "...</kbd>", "..." };
                yield return new object[] { "<li>...</li>", "<li>...</li>" };
                yield return new object[] { "<ol>...</ol>", "<ol>...</ol>" };
                yield return new object[] { "<ol>...", "..." };
                yield return new object[] { "...</ol>", "..." };
                yield return new object[] { "<p>...</p>", "<p>...</p>" };
                yield return new object[] { "<pre>...</pre>", "<pre>...</pre>" };
                yield return new object[] { "<pre><code>...</code></pre>", "<pre><code>...</code></pre>" };
                yield return new object[] { "<pre>...", "..." };
                yield return new object[] { "...</pre>", "..." };
                yield return new object[] { "<s>...</s>", "<s>...</s>" };
                yield return new object[] { "<s>...", "..." };
                yield return new object[] { "...</s>", "..." };
                yield return new object[] { "<sup>...</sup>", "<sup>...</sup>" };
                yield return new object[] { "<sup>...", "..." };
                yield return new object[] { "...</sup>", "..." };
                yield return new object[] { "<sub>...</sub>", "<sub>...</sub>" };
                yield return new object[] { "<sub>...", "..." };
                yield return new object[] { "...</sub>", "..." };
                yield return new object[] { "<strong>...</strong>", "<strong>...</strong>" };
                yield return new object[] { "<strong>...", "..." };
                yield return new object[] { "...</strong>", "..." };
                yield return new object[] { "<strike>...</strike>", "<strike>...</strike>" };
                yield return new object[] { "<strike>...", "..." };
                yield return new object[] { "...</strike>", "..." };
                yield return new object[] { "<ul>...</ul>", "<ul>...</ul>" };
                yield return new object[] { "<ul>...", "..." };
                yield return new object[] { "...</ul>", "..." };

                yield return new object[] { "<div>...</div>", "..." };
                yield return new object[] { "<pre class=\"prettyprint\">...</pre>", "..." };
                yield return new object[] { "<span>...</span>", "..." };
                yield return new object[] { "<script>...</script>", "..." };
                yield return new object[] { "</script>", "" };
            }
        }
    }
}
