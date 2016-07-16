using FluentAssertions;
using NUnit.Framework;
using System.Collections;

namespace Blongo.Tests
{
    public class SlugTests
    {
        [TestCaseSource("UrlSlugTestData")]
        public void UrlSlug_UrlSlug(string value, string expectedUrlSlug)
        {
            var urlSlug = new UrlSlug(value);

            urlSlug.Value.Should().Be(expectedUrlSlug);
        }

        public static IEnumerable UrlSlugTestData
        {
            get
            {
                yield return new object[] { "foo", "foo" };
                yield return new object[] { "FOO", "foo" };
                yield return new object[] { "-foo-", "foo" };
                yield return new object[] { "--foo--", "foo" };
                yield return new object[] { "foo bar baz", "foo-bar-baz" };
                yield return new object[] { "foo-bar-baz", "foo-bar-baz" };
                yield return new object[] { "foo--bar--baz", "foo-bar-baz" };
                yield return new object[] { "foo_bar_baz", "foo-bar-baz" };
                yield return new object[] { "foo__bar__baz", "foo-bar-baz" };
                yield return new object[] { "abcdefghijklmnopqrstuvwxyz0123456789-'.!()[]*<>", "abcdefghijklmnopqrstuvwxyz0123456789-'.!()[]*<>" };
            }
        }
    }
}
