namespace NbuReservationSystem.Web.Models.Responses.Reservations
{
    using System.Collections.Generic;

    using NbuReservationSystem.Web.Models.Requests.Reservations;

    public class MonthlyReservationsViewModel
    {
        public MonthlyReservationsViewModel(IList<WeekViewModel> weeks, int year, int month, string hall)
        {
            this.Weeks = weeks;
            this.Year = year;
            this.Month = month;
            this.Hall = hall;
        }

        public IList<WeekViewModel> Weeks { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string Hall { get; set; }
    }
}