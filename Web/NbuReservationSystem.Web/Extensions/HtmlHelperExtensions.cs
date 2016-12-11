namespace NbuReservationSystem.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;

    using NbuReservationSystem.Web.Models.Responses.Reservations;
    using NbuReservationSystem.Web.Models.Util;

    public static class HtmlHelperExtensions
    {
        private static readonly Dictionary<Month, Func<string>> MonthResources;

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
    }
}
