namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System.ComponentModel.DataAnnotations;

    public class OrganizerViewModel
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
