using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blongo
{
    public static class DateTimeExtensions
    {
        public static string RelativeTo(this DateTime extended, DateTime now)
        {
            if (extended == now)
            {
                return "now";
            }

            var delta = new TimeSpan(Math.Abs(now.Ticks - extended.Ticks));
            var deltaSeconds = delta.TotalSeconds;
            bool isPast = now.Ticks > extended.Ticks;
            string agoFromNow = isPast ? "ago" : "from now";

            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int week = day * 7;

            if (deltaSeconds < 5 * second)
            {
                return $"a moment {agoFromNow}";
            }

            if (deltaSeconds < 1 * minute)
            {
                return $"{delta.Seconds} seconds {agoFromNow}";
            }

            if (deltaSeconds < 1 * minute + 30 * second)
            {
                return $"a minute {agoFromNow}";
            }

            if (deltaSeconds < 45 * minute)
            {
                return $"{Math.Round(delta.TotalMinutes)} minutes {agoFromNow}";
            }

            if (deltaSeconds < 1 * hour + 30 * minute)
            {
                return $"an hour {agoFromNow}";
            }

            if (deltaSeconds < 12 * hour)
            {
                return $"{Math.Round(delta.TotalHours)} hours {agoFromNow}";
            }

            if (extended.Day == now.Day && extended.Month == now.Month && extended.Year == now.Year)
            {
                if (extended.Hour < 9)
                {
                    return "early this morning";
                }

                if (extended.Hour < 12)
                {
                    return "this morning";
                }

                if (extended.Hour < 16)
                {
                    return "this afternoon";
                }

                if (extended.Hour < 20)
                {
                    return "this evening";
                }

                if (extended.Hour < 22)
                {
                    return "tonight";
                }

                return "late tonight";
            }

            if ((extended.Day == now.Day - 1 || extended.Day == now.Day + 1) && extended.Month == now.Month && extended.Year == now.Year)
            {
                var yesterdayTomorrow = isPast ? "yesterday" : "tomorrow";

                if (extended.Hour < 9)
                {
                    return $"early {yesterdayTomorrow} morning";
                }

                if (extended.Hour < 12)
                {
                    return $"{yesterdayTomorrow} morning";
                }

                if (extended.Hour < 16)
                {
                    return $"{yesterdayTomorrow} afternoon";
                }

                if (extended.Hour < 20)
                {
                    return $"{yesterdayTomorrow} evening";
                }

                if (extended.Hour < 22)
                {
                    return isPast ? "last night" : "tomorrow night";
                }

                return isPast ? "late last night" : "late tomorrow night";
            }

            extended.DayOfWeek

            return extended.ToString("MMM d yyyy");
        }
    }
}
