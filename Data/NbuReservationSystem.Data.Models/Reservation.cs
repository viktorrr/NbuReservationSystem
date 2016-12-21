namespace NbuReservationSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common.Models;

    public class Reservation : BaseModel<int>
    {

        [Index("IX_ReservationUniqueness", 1, IsUnique = true)]
        public DateTime Date { get; set; }

        [Index("IX_ReservationUniqueness", 2, IsUnique = true)]
        public TimeSpan BeginsOn { get; set; }

        [Index("IX_ReservationUniqueness", 3, IsUnique = true)]
        public TimeSpan EndsOn { get; set; }

        public string Title { get; set; }

        public string Token { get; set; }

        public string Description { get; set; }

        public bool IsEquipementRequired { get; set; }

        public virtual int OrganaiserId { get; set; }

        public virtual Organiser Organiser { get; set; }

        public override string ToString()
        {
            return this.Token;
        }
    }
}
