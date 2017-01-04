namespace NbuReservationSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common.Models;

    public class Reservation : BaseModel<int>
    {

        [Index("IX_ReservationUniqueness", 1, IsUnique = true)]
        public DateTime Date { get; set; }

        [Index("IX_ReservationUniqueness", 2, IsUnique = true)]
        public TimeSpan StartHour { get; set; }

        [Index("IX_ReservationUniqueness", 3, IsUnique = true)]
        public TimeSpan EndHour { get; set; }

        [Required]
        public string Title { get; set; }

        public string Token { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Assignor { get; set; }

        public bool IsEquipementRequired { get; set; }

        [Required]
        public virtual int OrganaiserId { get; set; }

        [Required]
        public virtual Organiser Organiser { get; set; }

        public override string ToString()
        {
            return this.Token;
        }
    }
}
