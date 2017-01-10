namespace NbuReservationSystem.Services.Web
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web.Hosting;

    using NbuReservationSystem.Web.Models.Requests.Reservations;

    public class EmailService : IEmailService
    {
        private static readonly string Username;
        private static readonly string Password;
        private static readonly string NewReservationTemplate;
        private static readonly string ForgottenPasswordTemplate;

        static EmailService()
        {
            Username = ConfigurationManager.AppSettings["Email"];
            Password = ConfigurationManager.AppSettings["Password"];
            NewReservationTemplate = File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/NewReservation.txt"));
            ForgottenPasswordTemplate = File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/ForgottenPassword.txt"));
        }

        public void SendNewReservationEmail(ReservationViewModel model, string token)
        {
            var subject = $"Нова резервация / New reservation - {FormatDate(model.Date)}";
            var body = FormatNewReservationEmail(model, token);

            Task.Run(() => Send(model.Organizer.Email, subject, body));
        }

        public void SendForgottenPasswordEmail(string receiver, string url)
        {
            var subject = "Забравена парола / Forgotten password";
            var body = FormatForgottenPasswordEmail(url);

            Task.Run(() => Send(receiver, subject, body));
        }

        private static void Send(string receiver, string subject, string body)
        {
            try
            {
                var msg = new MailMessage
                {
                    From = new MailAddress(Username),
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                };

                msg.To.Add(receiver);

                using (var client = new SmtpClient())
                {
                    client.Credentials = new NetworkCredential(Username, Password);
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(Username, Password);
                    client.Timeout = 20000;

                    // workaround for NBU's mail server - who needs security anyway?
                    // ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    client.Send(msg);
                }
            }
            catch (Exception)
            {
                // TODO: log this!
            }
        }

        private static string FormatNewReservationEmail(ReservationViewModel model, string token)
        {
            var date = FormatDate(model.Date);

            return string.Format(
                NewReservationTemplate,
                model.Organizer.Name,
                model.Title,
                date,
                model.StartHour,
                model.EndHour,
                token
            );
        }

        private static string FormatForgottenPasswordEmail(string url)
        {
            return string.Format(ForgottenPasswordTemplate, url);
        }

        private static string FormatDate(DateTime date)
        {
            return $"{date.Year}-{date.Month}-{date.Day}";
        }
    }
}
