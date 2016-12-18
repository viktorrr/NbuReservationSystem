namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddReservationViewModel
    {
        // TODO: validation messages!

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan BeginsOn { get; set; }

        public TimeSpan EndsOn { get; set; }

        public bool IsEquipmentRequired { get; set; }

        public string Assignor { get; set; }

        public OrganizerViewModel Organizer { get; set; }

        public RepetitionPolicy RepetitionPolicy { get; set; }
    }
}
