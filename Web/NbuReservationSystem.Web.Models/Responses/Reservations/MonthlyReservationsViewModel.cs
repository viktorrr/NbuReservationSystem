namespace NbuReservationSystem.Web.Models.Responses.Reservations
{
    using System.Collections.Generic;

    using Util;

    public class MonthlyReservationsViewModel
    {
        public IList<WeekViewModel> Weeks { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }
    }
}