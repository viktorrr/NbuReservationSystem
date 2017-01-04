namespace NbuReservationSystem.Services.Web
{
    using System;
    using System.Collections.Generic;

    using NbuReservationSystem.Web.Models.Enums;
    using NbuReservationSystem.Web.Models.Requests.Reservations;

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
        private static readonly Dictionary<DayOfWeek, int> DayOfWeekToInt;

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

            DayOfWeekToInt = new Dictionary<DayOfWeek, int>
            {
                { DayOfWeek.Monday, 0 },
                { DayOfWeek.Tuesday, 1 },
                { DayOfWeek.Wednesday, 2 },
                { DayOfWeek.Thursday, 3 },
                { DayOfWeek.Friday, 4 },
                { DayOfWeek.Saturday, 5 },
                { DayOfWeek.Sunday, 6 }
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

        public IEnumerable<DateTime> CalculateDates(ReservationViewModel model)
        {
            var result = new LinkedList<DateTime>();

            // add the explicitly specified start date
            result.AddLast(model.Date);

            if (model.RepetitionPolicy.RepetitionType == RepetitionType.OneTimeOnly)
            {
                return result;
            }

            // step 1: find out in which day the given event should occur
            // e.g monday and friday
            var weekDaysToRepeat = CalculateWeekdaysToRepeat(model);

            // step 2: calculate the "window" between 2 occurrences
            // e.g: 2016-01-01 -> 2016-01-22 which occurs every 2 weeks
            var daysToSkip = CalculateDaysToSkip(model);

            // find out the first day of the week of the specified beginning date
            // and add the days of the current week to the result
            var startDate = GetMondayOfWeek(model.Date);

            if (model.RepetitionPolicy.RepetitionType == RepetitionType.EndAfterExactNumberOfRepetitions)
            {
                CalculateForExactNumberOfRepetitions(model, startDate, daysToSkip, weekDaysToRepeat, result);
            }

            if (model.RepetitionPolicy.RepetitionType == RepetitionType.EndOnSpecificDate)
            {
                CalculateForExactEndDate(model, startDate, daysToSkip, weekDaysToRepeat, result);
            }

            if (model.RepetitionPolicy.RepetitionType == RepetitionType.Forever)
            {
                // so.. just generate reservations for the next
                // 7 years... because 7 is a nice number?

                var endDate = model.Date.AddYears(7);
                model.RepetitionPolicy.EndDate = endDate;
                CalculateForExactEndDate(model, startDate, daysToSkip, weekDaysToRepeat, result);
            }

            return result;
        }

        private static IList<int> CalculateWeekdaysToRepeat(ReservationViewModel model)
        {
            var weekDaysToRepeat = new List<int>();

            for (int i = 0; i < model.RepetitionPolicy.RepetitionDays.Count; i++)
            {
                if (model.RepetitionPolicy.RepetitionDays[i])
                {
                    weekDaysToRepeat.Add(i);
                }
            }

            return weekDaysToRepeat;
        }

        private static int CalculateDaysToSkip(ReservationViewModel model)
        {
            var daysToSkip = 7;
            if (model.RepetitionPolicy.RepetitionWindow.HasValue)
            {
                // we need to add 1 .. otherwise we'd end up with less reservations -
                // the week of the initial reservation does not count!
                var weeksWindow = model.RepetitionPolicy.RepetitionWindow.Value + 1;
                daysToSkip = daysToSkip * weeksWindow;
            }

            return daysToSkip;
        }

        private static void CalculateForExactEndDate(
            ReservationViewModel model,
            DateTime startDate,
            int daysToSkip,
            IList<int> weekDaysToRepeat,
            LinkedList<DateTime> list)
        {
            var endDate = model.RepetitionPolicy.EndDate;
            var currentDate = startDate;

            foreach (int weekDay in weekDaysToRepeat)
            {
                // check if the selected day is not in the past
                // e.g: selected day is friday, repeating days are monday and friday ->
                // monday is in the past -> add only friday
                if (DayOfWeekToInt[model.Date.DayOfWeek] < weekDay)
                {
                    currentDate = startDate.AddDays(weekDay);
                    if (currentDate > endDate)
                    {
                        return;
                    }

                    list.AddLast(currentDate);
                }
            }

            if (currentDate == endDate)
            {
                return;
            }

            // continue with the weeks in the future..
            while (true)
            {
                startDate = startDate.AddDays(daysToSkip);

                foreach (var day in weekDaysToRepeat)
                {
                    currentDate = startDate.AddDays(day);
                    if (currentDate > endDate)
                    {
                        return;
                    }

                    list.AddLast(currentDate);
                }
            }
        }

        private static void CalculateForExactNumberOfRepetitions(
            ReservationViewModel model,
            DateTime startDate,
            int daysToSkip,
            IList<int> weekDaysToRepeat,
            LinkedList<DateTime> list)
        {
            // find out the first day of the week of the specified beginning date
            // and add the days of the current week to the result
            foreach (int weekDay in weekDaysToRepeat)
            {
                // check if the selected day is not in the past
                // e.g: selected day is friday, repeating days are monday and friday ->
                // monday is in the past -> add only friday
                if (DayOfWeekToInt[model.Date.DayOfWeek] < weekDay)
                {
                    var dayToAdd = startDate.AddDays(weekDay);
                    list.AddLast(dayToAdd);
                }
            }

            startDate = startDate.AddDays(daysToSkip);
            if (!model.RepetitionPolicy.Repetitions.HasValue)
            {
                return;
            }

            // continue with the weeks in the future..
            for (int i = 0; i < model.RepetitionPolicy.Repetitions.Value - 1; i++)
            {
                foreach (var day in weekDaysToRepeat)
                {
                    var dayToAdd = startDate.AddDays(day);
                    list.AddLast(dayToAdd);
                }
                startDate = startDate.AddDays(daysToSkip);
            }
        }

        private static DateTime GetMondayOfWeek(DateTime date)
        {
            var daysToRemove = DayOfWeekToInt[date.DayOfWeek] * -1;
            return date.AddDays(daysToRemove);
        }
    }
}