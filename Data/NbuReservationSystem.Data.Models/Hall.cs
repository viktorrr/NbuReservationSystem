namespace NbuReservationSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NbuReservationSystem.Data.Common.Models;

    public class Hall : BaseModel<int>
    {
        public Hall()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }

        public virtual HashSet<Reservation> Reservations { get; set; }
    }
}
