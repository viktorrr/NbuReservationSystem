namespace NbuReservationSystem.Services.Web
{
    using System;

    using NbuReservationSystem.Web.Models.Requests.Reservations;
    using NbuReservationSystem.Web.Models.Responses.Reservations;

    public interface IReservationsService
    {
        MonthlyReservationsViewModel GetReservations(int year, int month, int hallId);

        DayViewModel GetReservations(DateTime date, int hallId);

        int AddReservations(ReservationViewModel reservationViewModel, string ip);
    }
}