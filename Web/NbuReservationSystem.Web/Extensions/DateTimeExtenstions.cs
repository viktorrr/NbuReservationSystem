namespace NbuReservationSystem.Web.Extensions
{
    using System;
    using System.Globalization;

    public static class DateTimeExtenstions
    {
        public static string DayTabUrl(this DateTime source)
        {
            return source.Year.ToString(CultureInfo.InvariantCulture) + "-"
                   + source.Month.ToString(CultureInfo.InvariantCulture) + "-"
                   + source.Day.ToString(CultureInfo.InvariantCulture);
        }
    }
}
