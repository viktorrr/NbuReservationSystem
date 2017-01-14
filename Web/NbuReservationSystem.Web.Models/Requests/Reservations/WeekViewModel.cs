namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System.Collections.Generic;

    using NbuReservationSystem.Web.Models.Responses.Reservations;

    public class WeekViewModel
    {
        public IEnumerable<DayViewModel> Days { get; set; }

        public int Month { get; set; }

        public string Hall { get; set; }
    }
}