namespace NbuReservationSystem.Services.Web
{
    using NbuReservationSystem.Web.Models.Requests.Reservations;

    public interface IEmailService
    {
        void SendEmail(ReservationViewModel model, string token);
    }
}