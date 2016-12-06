namespace NbuReservationSystem.Services.Web
{
    using System;

    using NbuReservationSystem.Web.ViewModels.Reservations;

    public interface IReservationsService
    {
        MonthlyReservationsViewModel GetReservations(int year, int month);

        DayViewModel GetReservations(DateTime date);
    }
}