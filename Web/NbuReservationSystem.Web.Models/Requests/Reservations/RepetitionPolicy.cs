namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Enums;

    using NbuReservationSystem.Web.Models.LocalizationResources.Reservations;

    public class RepetitionPolicy
    {
        public RepetitionPolicy()
        {
            this.RepetitionDays = new List<bool>();
        }

        [Range(0, int.MaxValue)]
        public int? RepetitionWindow { get; set; }

        [Range(0, int.MaxValue)]
        public int? Repetitions { get; set; }

        public IList<bool> RepetitionDays { get; set; }

        public DateTime EndDate { get; set; }

        public CancellationType? CancellationType { get; set; }
    }
}
