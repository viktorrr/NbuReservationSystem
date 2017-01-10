namespace NbuReservationSystem.Web.Models.Requests.Account
{
    using System.ComponentModel.DataAnnotations;

    using NbuReservationSystem.Web.Models.LocalizationResources;

    public class LoginViewModel
    {
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(ValidationMessages))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
