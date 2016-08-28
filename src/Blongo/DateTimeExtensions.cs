namespace Blongo
{
    using System;

    public static class DateTimeExtensions
    {
        public static string RelativeTo(this DateTime extended, DateTime now)
        {
            var delta = new TimeSpan(Math.Abs(now.Ticks - extended.Ticks));
            var deltaSeconds = delta.TotalSeconds;
            var isInThePast = now.Ticks > extended.Ticks;
            var agoFromNow = isInThePast ? "ago" : "from now";

            const int second = 1;
            const int minute = 60*second;
            const int hour = 60*minute;

            if (deltaSeconds < 1*second)
            {
                return "just now";
            }

            if (deltaSeconds == 1*second)
            {
                return $"1 sec {agoFromNow}";
            }

            if (deltaSeconds < 1*minute)
            {
                return $"{delta.Seconds} secs {agoFromNow}";
            }

            if (deltaSeconds == 1*minute)
            {
                return $"1 min {agoFromNow}";
            }

            if (deltaSeconds < 1*hour)
            {
                return $"{Math.Min(59, Math.Ceiling(delta.TotalMinutes))} mins {agoFromNow}";
            }

            if (deltaSeconds == 1*hour)
            {
                return $"1 hr {agoFromNow}";
            }

            if (deltaSeconds <= 3*hour)
            {
                return $"{Math.Min(59, Math.Ceiling(delta.TotalHours))} hrs {agoFromNow}";
            }

            if (extended.Day == now.Day && extended.Month == now.Month && extended.Year == now.Year)
            {
                return $"Today at {extended.ToString("HH:mm")}";
            }

            if (Math.Ceiling(delta.TotalDays) == 1)
            {
                return $"{(isInThePast ? "Yesterday" : "Tomorrow")} at {extended.ToString("HH:mm")}";
            }

            var daysInMonths = DateTime.DaysInMonth(extended.Year, extended.Month);

            if (Math.Ceiling(delta.TotalDays) <= daysInMonths)
            {
                if (extended.Year == now.Year)
                {
                    return extended.ToString("d MMM 'at' HH:mm");
                }

                return extended.ToString("d MMM yyyy 'at' HH:mm");
            }

            if (extended.Year == now.Year)
            {
                return extended.ToString("d MMM");
            }

            return extended.ToString("d MMM yyyy");
        }
    }
}