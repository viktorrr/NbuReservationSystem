namespace NbuReservationSystem.Web.Models.Responses.Reservations
{
    using System;
    using System.Collections.Generic;

    using NbuReservationSystem.Data.Models;

    public class DayViewModel
    {
        public IList<Reservation> Reservations { get; set; }

        public DateTime Day { get; set; }

        public string Hall { get; set; }
    }
}