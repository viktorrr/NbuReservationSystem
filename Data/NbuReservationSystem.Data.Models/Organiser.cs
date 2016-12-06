namespace NbuReservationSystem.Data.Models
{
    using System.Collections.Generic;

    using Common.Models;

    public class Organiser : BaseModel<int>
    {
        public Organiser()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        // TODO: Consider moving this to BaseModel!
        public string DeletedBy { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
