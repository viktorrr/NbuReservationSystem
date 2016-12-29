namespace NbuReservationSystem.Services.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data;

    using NbuReservationSystem.Data.Models;
    using NbuReservationSystem.Web.Models.Requests.Reservations;
    using NbuReservationSystem.Web.Models.Responses.Reservations;
    using NbuReservationSystem.Web.Models.Util;

    public class ReservationsService : IReservationsService
    {
        private readonly IReservationsDataService reservationsDataService;
        private readonly ICalendarService calendarService;

        public ReservationsService(IReservationsDataService reservationsDataService, ICalendarService calendarService)
        {
            this.reservationsDataService = reservationsDataService;
            this.calendarService = calendarService;
        }

        public void AddReservations(ReservationViewModel model)
        {
            // TODO: implement me
            // TODO: calculate dates -> save a new entry to the db
            // TODO: catch a System.Data.Entity.Infrastructure.DbUpdateException from EF -> notify the user the date is taken
        }

        public MonthlyReservationsViewModel GetReservations(int year, int month)
        {
            var now = new DateTime(year, month, 1).ToUniversalTime().AddHours(2);

            var from = this.calendarService.GetFirstDayOfMonthView(now.Year, now.Month);
            var to = this.calendarService.GetLastDayOfMonthView(now.Year, now.Month);

            var reservationsByDay = this.reservationsDataService.GetReservations(from, to);
            var dayIndex = from;

            var weeks = new List<WeekViewModel>(5);
            for (int i = 0; i < 5; i++)
            {
                var days = new List<DayViewModel>();

                for (int j = 0; j < 7; j++)
                {
                    var reservations = new List<Reservation>();
                    if (reservationsByDay.ContainsKey(dayIndex))
                    {
                        reservations = reservationsByDay[dayIndex].ToList();
                    }

                    var day = new DayViewModel
                    {
                        Reservations = reservations,
                        Day = dayIndex
                    };

                    dayIndex = dayIndex.AddDays(1);
                    days.Add(day);
                }

                var week = new WeekViewModel { Days = days, Month = now.Month };
                weeks.Add(week);
            }

            return new MonthlyReservationsViewModel
            {
                Weeks = weeks,
                Year = year,
                Month = month
            };
        }

        public DayViewModel GetReservations(DateTime date)
        {
            var reservations = this.reservationsDataService.GetReservations(date).ToList();

            return new DayViewModel { Day = date, Reservations = reservations };
        }
    }
}
