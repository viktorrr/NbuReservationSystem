namespace NbuReservationSystem.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web.Mvc;

    using Models.Enums;
    using Models.Requests.Reservations;
    using Models.Responses.Reservations;

    using NbuReservationSystem.Web.App_GlobalResources.Labels;
    using NbuReservationSystem.Web.App_GlobalResources.Reservations;

    public static class HtmlHelperExtensions
    {
        // TODO: most of these functions / dictionaries should live somewhere else!

        private static readonly string DefaultCulture = "bg";
        private static readonly Dictionary<Month, Func<string>> MonthResources;
        private static readonly Dictionary<Day, Func<string>> DayResources;

        static HtmlHelperExtensions()
        {
            MonthResources = new Dictionary<Month, Func<string>>
            {
                [Month.January] = () => Months.January,
                [Month.February] = () => Months.February,
                [Month.March] = () => Months.March,
                [Month.April] = () => Months.April,
                [Month.May] = () => Months.May,
                [Month.July] = () => Months.July,
                [Month.June] = () => Months.June,
                [Month.August] = () => Months.August,
                [Month.September] = () => Months.September,
                [Month.October] = () => Months.October,
                [Month.November] = () => Months.November,
                [Month.December] = () => Months.December,
            };

            DayResources = new Dictionary<Day, Func<string>>
            {
                [Day.Monday] = () => Weekdays.Monday,
                [Day.Tuesday] = () => Weekdays.Tuesday,
                [Day.Wednesday] = () => Weekdays.Wednesday,
                [Day.Thursday] = () => Weekdays.Thursday,
                [Day.Friday] = () => Weekdays.Friday,
                [Day.Saturday] = () => Weekdays.Saturday,
                [Day.Sunday] = () => Weekdays.Sunday,
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
                return $"{model.Reservations.Count} { ReservationLabels.SingleReservation }";
            }

            return $"{model.Reservations.Count} { ReservationLabels.MultipleReservations }";
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

            return new { year, month, hallName = model.Hall };
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

            return new { year, month, hallName = model.Hall };
        }

        public static SelectList CreateDaysSelectList(this HtmlHelper helper, ReservationViewModel viewModel)
        {
            var enumValues = Enum.GetValues(typeof(Day))
                .Cast<Day>()
                .Select(e => new { Value = e.ToString(), Text = e.ToString() })
                .ToList();

            return new SelectList(enumValues, "Value", "Text", string.Empty);
        }

        public static string GetDataTablesCulture(this HtmlHelper helper)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            return currentCulture == "en" ? currentCulture : DefaultCulture;
        }
    }
}
