namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Enums;

    public class RepetitionPolicy
    {
        public int? RepetitionWindow { get; set; }

        public int? Repetitions { get; set; }

        [MaxLength(7)]
        public IList<bool> RepetitionDays { get; set; }

        public DateTime EndDate { get; set; }

        public CancellationType? CancellationType { get; set; }
    }
}
