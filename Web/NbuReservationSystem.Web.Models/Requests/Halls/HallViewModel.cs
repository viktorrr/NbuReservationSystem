namespace NbuReservationSystem.Web.Models.Requests.Halls
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using NbuReservationSystem.Web.Models.LocalizationResources;

    public class HallViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        [StringLength(100, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ValidationMessages), MinimumLength = 4)]
        public string Name { get; set; }

        [RegularExpression("^(#[A-Fa-f0-9]{6})$", ErrorMessageResourceName = "InvalidColor", ErrorMessageResourceType = typeof(ValidationMessages))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Color { get; set; }

        [AllowHtml]
        public string GoogleCalendarContent { get; set; }
    }
}
