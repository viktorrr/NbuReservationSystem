namespace NbuReservationSystem.Web.ViewModels.Reservations
{
    using System;
    using System.Collections.Generic;

    using Data.Models;

    public class DayViewModel
    {
        public IList<Reservation> Reservations { get; set; }

        public DateTime Day { get; set; }
    }
}