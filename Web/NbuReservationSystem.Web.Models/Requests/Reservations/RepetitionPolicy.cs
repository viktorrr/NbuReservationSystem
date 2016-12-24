namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System;
    using System.Collections.Generic;

    using Enums;

    public class RepetitionPolicy
    {
        public RepetitionPolicy()
        {
            this.RepetitionDays = new List<bool>();
        }

        public int? RepetitionWindow { get; set; }

        public int? Repetitions { get; set; }

        public IList<bool> RepetitionDays { get; set; }

        public DateTime EndDate { get; set; }

        public CancellationType? CancellationType { get; set; }
    }
}
