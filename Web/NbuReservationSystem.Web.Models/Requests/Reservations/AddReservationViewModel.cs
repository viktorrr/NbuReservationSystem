namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddReservationViewModel
    {
        // TODO: validation messages!

        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan BeginsOn { get; set; }

        [Required]
        public TimeSpan EndsOn { get; set; }

        public bool IsEquipmentRequired { get; set; }

        [Required]
        [StringLength(30)]
        public string Assignor { get; set; }

        [Required]
        public OrganizerViewModel Organizer { get; set; }

        public RepetitionPolicy RepetitionPolicy { get; set; }
    }
}
