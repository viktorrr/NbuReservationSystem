namespace NbuReservationSystem.Web.Models.Responses.Reservations
{
    using System;

    public class ReservationsAdministrationViewModel
    {
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartHour { get; set; }

        public TimeSpan EndHour { get; set; }

        public string Assignor { get; set; }

        public string Organizer { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string IP { get; set; }

        public string Hall { get; set; }

        public bool Equipment { get; set; }

        public int Id { get; set; }

        public bool Deleted { get; set; }
    }
}
