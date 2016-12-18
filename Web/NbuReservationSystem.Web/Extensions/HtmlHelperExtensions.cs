namespace NbuReservationSystem.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    using Models.Enums;
    using Models.Requests.Reservations;
    using Models.Responses.Reservations;

    public static class HtmlHelperExtensions
    {
        // TODO: most of these functions / dictionaries
        // should live somewhere else!

        private static readonly Dictionary<Month, Func<string>> MonthResources;

        private static readonly Dictionary<Day, Func<string>> DayResources;

        static HtmlHelperExtensions()
        {
            MonthResources = new Dictionary<Month, Func<string>>
            {
                [Month.January] = () => Resources.Months.January,
                [Month.February] = () => Resources.Months.February,
                [Month.March] = () => Resources.Months.March,
                [Month.April] = () => Resources.Months.April,
                [Month.May] = () => Resources.Months.May,
                [Month.July] = () => Resources.Months.July,
                [Month.June] = () => Resources.Months.June,
                [Month.August] = () => Resources.Months.August,
                [Month.September] = () => Resources.Months.September,
                [Month.October] = () => Resources.Months.October,
                [Month.November] = () => Resources.Months.November,
                [Month.December] = () => Resources.Months.December,
            };

            DayResources = new Dictionary<Day, Func<string>>
            {
                [Day.Monday] = () => Resources.Weekdays.Monday,
                [Day.Tuesday] = () => Resources.Weekdays.Tuesday,
                [Day.Wednesday] = () => Resources.Weekdays.Wednesday,
                [Day.Thursday] = () => Resources.Weekdays.Thursday,
                [Day.Friday] = () => Resources.Weekdays.Friday,
                [Day.Saturday] = () => Resources.Weekdays.Saturday,
                [Day.Sunday] = () => Resources.Weekdays.Sunday,
            };
        }

        public static string FormatEventDuration(this HtmlHelper helper, TimeSpan beginsOn, TimeSpan endsOn)
        {
            var sb = new StringBuilder();

            sb.Append($"{beginsOn.Hours:D2}:{beginsOn.Minutes:D2}");
            sb.Append(" - ");
            sb.Append($"{endsOn.Hours:D2}:{endsOn.Minutes:D2}");

            return sb.ToString();
        }

        public static string FormatEventStart(this HtmlHelper helper, TimeSpan beginsOn)
        {
            return $"{beginsOn.Hours:D2}:{beginsOn.Minutes:D2}";
        }

        public static string FormatLocalizedMonth(this HtmlHelper helper, MonthlyReservationsViewModel model)
        {
            return $"{MonthResources[(Month)model.Month]()}, {model.Year}";
        }

        public static string FormatLocalizedDay(this HtmlHelper helper, object day)
        {
            return $"{DayResources[(Day)day]()}";
        }

        public static string FormatDailyResevations(this HtmlHelper helper, DayViewModel model)
        {
            if (model.Reservations.Count == 1)
            {
                return $"{model.Reservations.Count} {Resources.Labels.SingleReservation}";
            }

            return $"{model.Reservations.Count} {Resources.Labels.MultipleReservations}";
        }

        public static int CalculateNextCalendarMonth(this HtmlHelper helper, int month)
        {
            return month <= 11 ? month + 1 : 1;
        }

        public static int CalculateNextCalendarYear(this HtmlHelper helper, int year, int month)
        {
            return month <= 11 ? year : year + 1;
        }

        public static object CreatePreviousMonthRoutValues(this HtmlHelper helper, MonthlyReservationsViewModel model)
        {
            var year = model.Year;
            var month = model.Month;

            if (month == 1)
            {
                year--;
                month = 12;
            }
            else
            {
                month--;
            }

            return new { year, month };
        }

        public static object CreateNextMonthRoutValues(this HtmlHelper helper, MonthlyReservationsViewModel model)
        {
            var year = model.Year;
            var month = model.Month;

            if (month == 12)
            {
                year++;
                month = 1;
            }
            else
            {
                month++;
            }

            return new { year, month };
        }

        public static SelectList CreateDaysSelectList(this HtmlHelper helper, AddReservationViewModel viewModel)
        {
            var enumValues = Enum.GetValues(typeof(Day))
                .Cast<Day>()
                .Select(e => new { Value = e.ToString(), Text = e.ToString() })
                .ToList();

            return new SelectList(enumValues, "Value", "Text", string.Empty);
        }
    }
}
