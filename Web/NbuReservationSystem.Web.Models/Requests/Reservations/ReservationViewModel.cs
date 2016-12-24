namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using NbuReservationSystem.Web.Models.LocalizationResources.Reservations;

    public class ReservationViewModel
    {
        private DateTime date;

        public ReservationViewModel()
        {
            this.RepetitionPolicy = new RepetitionPolicy();
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
