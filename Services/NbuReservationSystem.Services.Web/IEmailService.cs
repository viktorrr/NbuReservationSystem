namespace NbuReservationSystem.Services.Web
{
    using NbuReservationSystem.Web.Models.Requests.Reservations;

    public interface IEmailService
    {
        void SendNewReservationEmail(ReservationViewModel model, string token);

        void SendForgottenPasswordEmail(string receiver, string url);
    }
}