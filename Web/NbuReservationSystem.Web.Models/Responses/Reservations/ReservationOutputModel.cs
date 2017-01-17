namespace NbuReservationSystem.Web.Models.Responses.Reservations
{
    using System;

    public class ReservationOutputModel
    {
        public int Id { get; set; }

        public DateTime Day { get; set; }

        public TimeSpan StartHour { get; set; }

        public TimeSpan EndHour { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
