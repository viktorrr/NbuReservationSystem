namespace NbuReservationSystem.Web.Models.Requests.Account
{
    using System.ComponentModel.DataAnnotations;

    using NbuReservationSystem.Web.Models.LocalizationResources;

    public class RegisterViewModel
    {
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(ValidationMessages))]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        [StringLength(100, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(ValidationMessages), MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceName = "PasswordMissmatch", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string ConfirmPassword { get; set; }

        // TODO: add token!
    }
}
