namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using NbuReservationSystem.Web.Models.LocalizationResources.Reservations;

    public class AddReservationViewModel
    {
        // TODO: validation messages!

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public DateTime Date { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public TimeSpan BeginsOn { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public TimeSpan EndsOn { get; set; }

        public bool IsEquipmentRequired { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        [StringLength(30, ErrorMessageResourceName = "TooLongInput", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Assignor { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public OrganizerViewModel Organizer { get; set; }

        public RepetitionPolicy RepetitionPolicy { get; set; }
    }
}
