namespace NbuReservationSystem.Data.Models
{
    using System;

    using Common.Models;

    public class Reservation : BaseModel<int>
    {
        public DateTime Date { get; set; }

        public TimeSpan BeginsOn { get; set; }

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
