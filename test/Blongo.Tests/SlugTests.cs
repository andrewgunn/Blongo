namespace Blongo.Tests
{
    using System.Collections;
    using FluentAssertions;
    using NUnit.Framework;

    public class SlugTests
    {
        public static IEnumerable UrlSlugTestData
        {
            get
            {
                yield return new object[] {"foo", "foo"};
                yield return new object[] {"FOO", "foo"};
                yield return new object[] {"-foo-", "foo"};
                yield return new object[] {"--foo--", "foo"};
                yield return new object[] {"foo bar baz", "foo-bar-baz"};
                yield return new object[] {"foo-bar-baz", "foo-bar-baz"};
                yield return new object[] {"foo--bar--baz", "foo-bar-baz"};
                yield return new object[] {"foo_bar_baz", "foo-bar-baz"};
                yield return new object[] {"foo__bar__baz", "foo-bar-baz"};
                yield return
                    new object[]
                    {
                        "abcdefghijklmnopqrstuvwxyz0123456789-'.!()[]*<>",
                        "abcdefghijklmnopqrstuvwxyz0123456789-'.!()[]*<>"
                    };
            }
        }

        [TestCaseSource("UrlSlugTestData")]
        public void UrlSlug_Value(string input, string expected)
        {
            var urlSlug = new UrlSlug(input);

            urlSlug.Value.Should().Be(expected);
        }
    }
}