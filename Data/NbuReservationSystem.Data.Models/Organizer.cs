namespace NbuReservationSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common.Models;

    public class Organizer : BaseModel<int>
    {
        public Organizer()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string IP { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
