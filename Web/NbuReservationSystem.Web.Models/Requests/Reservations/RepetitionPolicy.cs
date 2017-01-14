namespace NbuReservationSystem.Web.Models.Requests.Reservations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Enums;

    public class RepetitionPolicy
    {
        public RepetitionPolicy()
        {
            this.RepetitionDays = Enumerable.Repeat(false, 7).ToList();
            this.RepetitionWindow = 0;
        }

        [Range(0, int.MaxValue)]
        public int? RepetitionWindow { get; set; }

        [Range(0, int.MaxValue)]
        public int? Repetitions { get; set; }

        public IList<bool> RepetitionDays { get; set; }

        public DateTime? EndDate { get; set; }

        public RepetitionType RepetitionType { get; set; }
    }
}
