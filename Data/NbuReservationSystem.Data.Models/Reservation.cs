namespace NbuReservationSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common.Models;

    public class Reservation : BaseModel<int>
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartHour { get; set; }

        [Required]
        public TimeSpan EndHour { get; set; }

        [Required]
        public string Title { get; set; }

        // TODO: required?
        public string Token { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Assignor { get; set; }

        public bool IsEquipementRequired { get; set; }

        [Required]
        public virtual int OrganizerId { get; set; }

        [Required]
        public virtual Organizer Organizer { get; set; }

        [Required]
        public virtual int HallId { get; set; }

        [Required]
        public virtual Hall Hall { get; set; }

        public override string ToString()
        {
            return this.Token;
        }
    }
}
