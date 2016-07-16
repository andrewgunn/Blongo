using FluentAssertions;
using NUnit.Framework;
using System.Collections;

namespace Blongo.Tests
{
    public class CommentHtmlStripperTests
    {
        [TestCaseSource("HtmlStripperTestData")]
        public void CommentHtmlStripper_StrippedHtml(string html, string expectedStrippedHtml)
        {
            var htmlStripper = new CommentHtmlStripper(html);
            var strippedHtml = htmlStripper.StrippedHtml;

            strippedHtml.Should().Be(expectedStrippedHtml);
        }

        public static IEnumerable HtmlStripperTestData
        {
            get
            {
                yield return new object[] { "<a>...</a>", "<a>...</a>" };
                yield return new object[] { "<a href=\"href\">...</a>", "<a href=\"href\">...</a>" };
                yield return new object[] { "<a href=\"href\" title=\"title\">...</a>", "<a href=\"href\" title=\"title\">...</a>" };
                yield return new object[] { "<b>...</b>", "<b>...</b>" };
                yield return new object[] { "<blockquote>...</blockquote>", "<blockquote>...</blockquote>" };
                yield return new object[] { "<code>...</code>", "<code>...</code>" };
                yield return new object[] { "<del>...</del>", "<del>...</del>" };
                yield return new object[] { "<dd>...</dd>", "<dd>...</dd>" };
                yield return new object[] { "<dl>...</dl>", "<dl>...</dl>" };
                yield return new object[] { "<dt>...</dt>", "<dt>...</dt>" };
                yield return new object[] { "<em>...</em>", "<em>...</em>" };
                yield return new object[] { "<h1>, <h2>, <h3>", "<h1>, <h2>, <h3>" };
                yield return new object[] { "<i>...</i>", "<i>...</i>" };
                yield return new object[] { "<i class=\"class\">", "<i class=\"class\">" };
                yield return new object[] { "<img>", "<img>" };
                yield return new object[] { "<img/>", "<img/>" };
                yield return new object[] { "<img />", "<img />" };
                yield return new object[] { "<img src=\"src\" />", "<img src=\"src\" />" };
                yield return new object[] { "<img src=\"src\" width=\"0\" />", "<img src=\"src\" width=\"0\" />" };
                yield return new object[] { "<img src=\"src\" width=\"0\" height=\"0\" />", "<img src=\"src\" width=\"0\" height=\"0\" />" };
                yield return new object[] { "<img src=\"src\" width=\"0\" height=\"0\" alt=\"alt\" />", "<img src=\"src\" width=\"0\" height=\"0\" alt=\"alt\" />" };
                yield return new object[] { "<img src=\"src\" width=\"0\" height=\"0\" alt=\"alt\" title=\"title\" />", "<img src=\"src\" width=\"0\" height=\"0\" alt=\"alt\" title=\"title\" />" };
                yield return new object[] { "</img>", "" };
                yield return new object[] { "<kbd>...</kbd>", "<kbd>...</kbd>" };
                yield return new object[] { "<li>...</li>", "<li>...</li>" };
                yield return new object[] { "<ol>...</ol>", "<ol>...</ol>" };
                yield return new object[] { "<p>...</p>", "<p>...</p>" };
                yield return new object[] { "<pre>...</pre>", "<pre>...</pre>" };
                yield return new object[] { "<pre class=\"prettyprint\">...</pre>", "<pre class=\"prettyprint\">...</pre>" };
                yield return new object[] { "<pre class=\"prettyprint linenums\">...</pre>", "<pre class=\"prettyprint linenums\">...</pre>" };
                yield return new object[] { "<s>...</s>", "<s>...</s>" };
                yield return new object[] { "<sup>...</sup>", "<sup>...</sup>" };
                yield return new object[] { "<sub>...</sub>", "<sub>...</sub>" };
                yield return new object[] { "<strong>...</strong>", "<strong>...</strong>" };
                yield return new object[] { "<strike>...</strike>", "<strike>...</strike>" };
                yield return new object[] { "<ul>...</ul>", "<ul>...</ul>" };
                yield return new object[] { "<br>", "<br>" };
                yield return new object[] { "<br/>", "<br/>" };
                yield return new object[] { "<br />", "<br />" };
                yield return new object[] { "</br>", "" };
                yield return new object[] { "<hr>", "<hr>" };
                yield return new object[] { "<hr/>", "<hr/>" };
                yield return new object[] { "<hr />", "<hr />" };
                yield return new object[] { "</hr>", "" };

                yield return new object[] { "<div>...</div>", "..." };
                yield return new object[] { "<span>...</span>", "..." };
                yield return new object[] { "<script>...</script>", "..." };
            }
        }
    }
}
