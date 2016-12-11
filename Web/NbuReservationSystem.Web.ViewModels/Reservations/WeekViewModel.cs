namespace NbuReservationSystem.Web.ViewModels.Reservations
{
    using System.Collections.Generic;

    public class WeekViewModel
    {
        public IEnumerable<DayViewModel> Days { get; set; }

        public int Month { get; set; }
    }
}