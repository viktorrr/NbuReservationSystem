namespace NbuReservationSystem.Services.Web
{
    using System;
    using System.Collections.Generic;

    using NbuReservationSystem.Web.Models.Requests.Reservations;

    /// <summary>
    /// The scheduler's default month view consists not only of the days in the current month,<para/>
    /// but also additional days (depending on which day of the week is the first day of the month <para/>
    /// and the last day of the month), which precede the 1st day of the month.<para/>
    /// <para>A simple service, which can make different dates-related calculations.</para>
    /// <see cref="CalendarService"/> is the default implementation
    /// </summary>
    public interface ICalendarService
    {
        DateTime GetFirstDayOfMonthView(int year, int month);

        DateTime GetLastDayOfMonthView(int year, int month);

        IEnumerable<DateTime> CalculateDates(ReservationViewModel model);
    }
}
