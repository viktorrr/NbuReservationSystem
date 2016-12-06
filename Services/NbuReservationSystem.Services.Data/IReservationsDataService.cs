namespace NbuReservationSystem.Services.Data
{
    using System;
    using System.Collections.Generic;

    using NbuReservationSystem.Data.Models;

    public interface IReservationsDataService
    {
        Dictionary<DateTime, IEnumerable<Reservation>> GetReservations(DateTime from, DateTime to);

        IEnumerable<Reservation> GetReservations(DateTime date);
    }
}