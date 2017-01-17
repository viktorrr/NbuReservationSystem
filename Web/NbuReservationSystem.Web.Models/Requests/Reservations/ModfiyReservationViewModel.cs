namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using NbuReservationSystem.Web.Models.LocalizationResources;

    public class ModfiyReservationViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Assignor { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Token { get; set; }

        public bool IsEquipmentRequired { get; set; }

        public bool ApplyToWholeSeries { get; set; }

        public bool Delete { get; set; }

        public OrganizerViewModel Organizer { get; set; }

        public int Id { get; set; }

        [ReadOnly(true)]
        public string Hall { get; set; }

        [ReadOnly(true)]
        public string Date { get; set; }

        [ReadOnly(true)]
        public string StartHour { get; set; }

        [ReadOnly(true)]
        public string EndHour { get; set; }
    }
}
