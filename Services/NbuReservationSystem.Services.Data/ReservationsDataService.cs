namespace NbuReservationSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NbuReservationSystem.Data.Common;
    using NbuReservationSystem.Data.Models;

    public class ReservationsDataService : IReservationsDataService
    {
        private readonly IRepository<Reservation> reservations;

        public ReservationsDataService(IRepository<Reservation> reservations)
        {
            this.reservations = reservations;
        }

        public Dictionary<DateTime, IEnumerable<Reservation>> GetReservations(DateTime @from, DateTime to)
        {
            var result =
                this.reservations.All()
                    .Where(x => x.Date >= from && x.Date <= to)
                    .OrderBy(x => x.Date)
                    .GroupBy(
                        x => x.Date,
                        (day, currentReservations) => new
                        {
                            day,
                            currentReservations
                        }).ToDictionary(x => x.day, x => x.currentReservations);
            return result;
        }

        public IEnumerable<Reservation> GetReservations(DateTime date)
        {
            return this.reservations.AllBy(x => x.Date == date);
        }
    }
}
