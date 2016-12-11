namespace NbuReservationSystem.Web.ViewModels.Reservations
{
    using System.Collections.Generic;

    public class MonthlyReservationsViewModel
    {
        public IList<WeekViewModel> Weeks { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }
    }
}