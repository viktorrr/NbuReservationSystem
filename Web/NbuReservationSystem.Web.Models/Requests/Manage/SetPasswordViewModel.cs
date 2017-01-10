﻿namespace NbuReservationSystem.Web.Models.Requests.Manage
{
    using System.ComponentModel.DataAnnotations;

    using NbuReservationSystem.Web.Models.LocalizationResources;

    public class SetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(ValidationMessages))]
        [StringLength(100, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(ValidationMessages), MinimumLength = 6)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordMissmatch", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string ConfirmPassword { get; set; }
    }
}
