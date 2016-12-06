namespace NbuReservationSystem.Services.Web
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// All results from this implementation return an UTC <see cref="DateTime"/> instances.
    /// <seealso cref="DateTime.KindUtc"/>
    /// </summary>
    public class CalendarService : ICalendarService
    {
        /// <summary>
        /// A month has between 28 and 31 days. This means that, starting from
        /// the first day of the month, we can add 27 days and continue adding
        /// +1 days until we get to the next month.
        /// </summary>
        private const int MinDaysToAddToGetNextMonth = 27;

        /// <summary>
        /// Depending on the day, we need to add 0-6 days before(or after) the
        /// 1st(or last) day of the month.
        /// <para/>
        /// Instead of having multiple if statements and calculating the result
        /// every time, we can pre-cache this information.
        /// </summary>
        private static readonly Dictionary<DayOfWeek, int> DaysToRemove;

        private static readonly Dictionary<DayOfWeek, int> DaysToAdd;

        static CalendarService()
        {
            DaysToRemove = new Dictionary<DayOfWeek, int>
            {
                { DayOfWeek.Monday, -7 },
                { DayOfWeek.Tuesday, -1 },
                { DayOfWeek.Wednesday, -2 },
                { DayOfWeek.Thursday, -3 },
                { DayOfWeek.Friday, -4 },
                { DayOfWeek.Saturday, -5 },
                { DayOfWeek.Sunday, -6 }
            };

            DaysToAdd = new Dictionary<DayOfWeek, int>
            {
                { DayOfWeek.Monday, 6 },
                { DayOfWeek.Tuesday, 5 },
                { DayOfWeek.Wednesday, 4 },
                { DayOfWeek.Thursday, 3 },
                { DayOfWeek.Friday, 2 },
                { DayOfWeek.Saturday, 1 },
                { DayOfWeek.Sunday, 0 }
            };
        }

        /// <summary>
        /// Calculates the Monday of the week in which is the first day of the month.<para/>
        /// <example>
        /// 2016-12-1 is Thursday. This method will return a DateTime instance,
        /// which is 3 days before 1st of December - 2016-11-28.
        /// </example>
        /// </summary>
        /// <returns>DateTime instance which is Monday and hh/mm/ss are 0.</returns>
        public DateTime GetFirstDayOfMonthView(int year, int month)
        {
            var beginningOfTheMonth = new DateTime(year, month, 1);

            var precedingDays = DaysToRemove[beginningOfTheMonth.DayOfWeek];
            var result = beginningOfTheMonth.AddDays(precedingDays);

            return DateTime.SpecifyKind(result, DateTimeKind.Utc);
        }

        /// <summary>
        /// Calculates the Sunday of the week in which is the last day of the month.<para/>
        /// <example>
        /// 2016-11-30 is Wednesday. This method will return a DateTime instance,
        /// which is 4 ahead of 30th of November - 2016-12-4 23:59:59
        /// </example>
        /// </summary>
        /// <returns>DateTime instance which is Sunday with time - 23:59:59.</returns>
        public DateTime GetLastDayOfMonthView(int year, int month)
        {
            var beginningOfTheMonth = new DateTime(year, month, 1);
            var result = beginningOfTheMonth.AddDays(MinDaysToAddToGetNextMonth);
            while (result.Month == month)
            {
                result = result.AddDays(1);
            }

            // go back to the last day of the month
            result = result.AddDays(-1);

            // and from that point in time - add the necessary days
            var daysToAdd = DaysToAdd[result.DayOfWeek];
            result = result.AddDays(daysToAdd);

            // add hh, min, sec
            result = new DateTime(result.Year, result.Month, result.Day, 23, 59, 59);

            return DateTime.SpecifyKind(result, DateTimeKind.Utc);
        }
    }
}
