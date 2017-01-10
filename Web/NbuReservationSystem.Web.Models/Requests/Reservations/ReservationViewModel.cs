namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using NbuReservationSystem.Web.Models.Enums;
    using NbuReservationSystem.Web.Models.LocalizationResources;

    public class ReservationViewModel
    {
        private DateTime date;

        public ReservationViewModel()
        {
            // by default assume this is a one-time only reservation
            // if its not - the user will choose one of the other 3..
            this.RepetitionPolicy = new RepetitionPolicy
            {
                RepetitionType = RepetitionType.OneTimeOnly
            };
        }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public DateTime Date
        {
            get { return this.date.Date; }
            set { this.date = value; }
        }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public TimeSpan StartHour { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public TimeSpan EndHour { get; set; }

        public bool IsEquipmentRequired { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        [StringLength(30, ErrorMessageResourceName = "TooLongInput", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Assignor { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public OrganizerViewModel Organizer { get; set; }

        public RepetitionPolicy RepetitionPolicy { get; set; }
    }
}
