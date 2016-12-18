namespace NbuReservationSystem.Web.Models.Util
{
    using System.Collections.Generic;

    using Responses.Reservations;

    public class WeekViewModel
    {
        public IEnumerable<DayViewModel> Days { get; set; }

        public int Month { get; set; }
    }
}