namespace NbuReservationSystem.Data.Models
{
    using System.Collections.Generic;

    using NbuReservationSystem.Data.Common.Models;

    public class Hall : BaseModel<int>
    {
        public Hall()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public string Name { get; set; }

        public string Color { get; set; }

        public virtual HashSet<Reservation> Reservations { get; set; }
    }
}
