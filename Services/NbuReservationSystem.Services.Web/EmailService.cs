namespace NbuReservationSystem.Services.Web
{
    using System;
    using System.Configuration;
    using System.Globalization;
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
        private static readonly string EmailTemplate;

        static EmailService()
        {
            Username = ConfigurationManager.AppSettings["Email"];
            Password = ConfigurationManager.AppSettings["Password"];
            EmailTemplate = File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/Email.txt"));
        }

        public void SendEmail(ReservationViewModel model, string token)
        {
            Task.Run(() => Send(model, token));
        }

        private static void Send(ReservationViewModel model, string token)
        {
            try
            {
                var msg = new MailMessage
                {
                    From = new MailAddress(Username),
                    IsBodyHtml = true,
                    Subject = $"Нова резервация / New reservation - {FormatDate(model.Date)}",
                    Body = FormatEmail(model, token)
                };

                msg.To.Add(model.Organizer.Email);

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

                    // TODO: complete disable of security is probably not the best idea..
                    // ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    client.Send(msg);
                }
            }
            catch (Exception)
            {
                // TODO: log this!
            }
        }

        private static string FormatEmail(ReservationViewModel model, string token)
        {
            var date = FormatDate(model.Date);

            return string.Format(
                EmailTemplate,
                model.Organizer.Name,
                model.Title,
                date,
                model.StartHour,
                model.EndHour,
                token
            );
        }

        private static string FormatDate(DateTime date)
        {
            return $"{date.Year}-{date.Month}-{date.Day}";
        }
    }
}
