namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System.ComponentModel.DataAnnotations;

    using NbuReservationSystem.Web.Models.LocalizationResources;

    public class OrganizerViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        [StringLength(30, ErrorMessageResourceName = "TooLongInput", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        [RegularExpression(@"^(\+359|0)[0-9]{9}$", ErrorMessageResourceName = "InvalidePhoneNumber", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Email { get; set; }
    }
}
