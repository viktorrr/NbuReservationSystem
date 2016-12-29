namespace NbuReservationSystem.Services.Web.Tests.TestData
{
    using System;

    using NbuReservationSystem.Web.Models.Requests.Reservations;

    public class CalculateDatesTestData
    {
        public ReservationViewModel Model { get; set; }

        public DateTime[] ExpectedDates { get; set; }
    }
}
