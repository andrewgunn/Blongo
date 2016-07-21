using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections;

namespace Blongo.Tests
{
    public class DateTimeExtensionsTests
    {
        [TestCaseSource("RelativeToTestData")]
        public void DateTimeExtensions_RelativeTo(DateTime extended, DateTime now, string expected)
        { 
            var relative = extended.RelativeTo(now);

            relative.Should().Be(expected);
        }

        public static IEnumerable RelativeToTestData
        {
            get
            {
                yield return new object[] { new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "now" };
                yield return new object[] { new DateTime(2000, 1, 1, 12, 00, 01, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "a moment from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 12, 00, 04, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "a moment from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 12, 00, 05, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "5 seconds from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 12, 00, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "59 seconds from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 12, 01, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "a minute from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 12, 01, 29, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "a minute from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 12, 01, 30, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 minutes from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 12, 44, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "45 minutes from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 12, 45, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "an hour from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 13, 29, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "an hour from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 13, 30, 000, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "2 hours from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 23, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "12 hours from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 23, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), "12 hours from now" };
                yield return new object[] { new DateTime(2000, 1, 1, 12, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "this afternoon" };
                yield return new object[] { new DateTime(2000, 1, 1, 15, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "this afternoon" };
                yield return new object[] { new DateTime(2000, 1, 1, 16, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "this evening" };
                yield return new object[] { new DateTime(2000, 1, 1, 19, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "this evening" };
                yield return new object[] { new DateTime(2000, 1, 1, 20, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "tonight" };
                yield return new object[] { new DateTime(2000, 1, 1, 21, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "tonight" };
                yield return new object[] { new DateTime(2000, 1, 1, 22, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "late tonight" };
                yield return new object[] { new DateTime(2000, 1, 1, 23, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "late tonight" };
                yield return new object[] { new DateTime(2000, 1, 2, 00, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "early tomorrow morning" };
                yield return new object[] { new DateTime(2000, 1, 2, 08, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "early tomorrow morning" };
                yield return new object[] { new DateTime(2000, 1, 2, 09, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "tomorrow morning" };
                yield return new object[] { new DateTime(2000, 1, 2, 11, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "tomorrow morning" };
                yield return new object[] { new DateTime(2000, 1, 2, 12, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "tomorrow afternoon" };
                yield return new object[] { new DateTime(2000, 1, 2, 15, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "tomorrow afternoon" };
                yield return new object[] { new DateTime(2000, 1, 2, 16, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "tomorrow evening" };
                yield return new object[] { new DateTime(2000, 1, 2, 19, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "tomorrow evening" };
                yield return new object[] { new DateTime(2000, 1, 2, 20, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "tomorrow night" };
                yield return new object[] { new DateTime(2000, 1, 2, 21, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "tomorrow night" };
                yield return new object[] { new DateTime(2000, 1, 2, 22, 00, 00, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "late tomorrow night" };
                yield return new object[] { new DateTime(2000, 1, 2, 23, 59, 59, DateTimeKind.Utc), new DateTime(2000, 1, 1, 00, 00, 00, DateTimeKind.Utc), "late tomorrow night" };
            }
        }
    }
}
