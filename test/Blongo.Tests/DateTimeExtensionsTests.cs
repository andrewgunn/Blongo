namespace Blongo.Tests
{
    using System;
    using System.Collections;
    using FluentAssertions;
    using NUnit.Framework;

    public class DateTimeExtensionsTests
    {
        public static IEnumerable RelativeToTestData
        {
            get
            {
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "just now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 12, 00, 00, 999, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "just now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 11, 59, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "1 sec ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 12, 00, 01, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "1 sec from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 11, 59, 58, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 secs ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 12, 00, 2, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 secs from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 11, 59, 01, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "59 secs ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 12, 00, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "59 secs from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 11, 59, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "1 min ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 12, 01, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "1 min from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 11, 58, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 mins ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 12, 01, 01, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 mins from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 11, 58, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 mins ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 12, 02, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 mins from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 11, 01, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "59 mins ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 12, 59, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "59 mins from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 11, 01, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "59 mins ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 12, 59, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "59 mins from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 11, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "1 hr ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 13, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "1 hr from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 10, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 hrs ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 13, 00, 01, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 hrs from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 10, 59, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 hrs ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 14, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 hrs from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 09, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "3 hrs ago"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 15, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "3 hrs from now"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 03, 01, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "Today at 03:01"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 1, 23, 59, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "Today at 23:59"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(1999, 12, 31, 00, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "Yesterday at 00:00"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 03, 00, 00, DateTimeKind.Utc), "Yesterday at 23:59"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 2, 00, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 20, 59, 59, DateTimeKind.Utc), "Tomorrow at 00:00"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 2, 23, 59, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 23, 59, 59, DateTimeKind.Utc), "Tomorrow at 23:59"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(1999, 12, 30, 23, 59, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "30 Dec 1999 at 23:59"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(1999, 12, 1, 00, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "1 Dec 1999 at 00:00"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 3, 00, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "3 Jan at 00:00"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 1, 31, 23, 59, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "31 Jan at 23:59"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(1999, 11, 30, 23, 59, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "30 Nov 1999"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 2, 1, 00, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "1 Feb"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2000, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "31 Dec"
                    };
                yield return
                    new object[]
                    {
                        new DateTime(2001, 1, 1, 00, 00, 00, DateTimeKind.Utc),
                        new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "1 Jan 2001"
                    };
            }
        }

        [TestCaseSource("RelativeToTestData")]
        public void DateTimeExtensions_RelativeTo(DateTime extended, DateTime now, string expected)
        {
            var relative = extended.RelativeTo(now);

            relative.Should().Be(expected);
        }
    }
}