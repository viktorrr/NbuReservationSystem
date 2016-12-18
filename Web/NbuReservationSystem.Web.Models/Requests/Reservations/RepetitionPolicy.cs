namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System.Collections.Generic;

    using Enums;

    public class RepetitionPolicy
    {
        public int? RepetitionWindow { get; set; }

        public int? Repetitions { get; set; }

        public IList<bool> RepetitionDays { get; set; }

        public string EndDate { get; set; }

        public CancellationType? CancellationType { get; set; }
    }
}
