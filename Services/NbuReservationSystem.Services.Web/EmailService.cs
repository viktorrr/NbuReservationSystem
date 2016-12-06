namespace NbuReservationSystem.Services.Web
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Mail;

    // TODO: implement this class
    public class EmailService
    {
        private static void SendEmail()
        {
            // TODO: this needs to be in the web project.......
            // TODO: add email + pw

            const string EmailAccount = "";
            const string Password = "";

            var msg = new MailMessage
            {
                From = new MailAddress(EmailAccount),
                Subject = "Hello world! " + DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Body = "hi to you ... :)"
            };

            // TODO: this needs to be an input..
            msg.To.Add("");

            using (var client = new SmtpClient())
            {
                // TODO: host
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(EmailAccount, Password);
                client.Timeout = 20000;

                // TODO: complete disable of security is probably not the best idea..
                // ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                client.Send(msg);
            }
        }
    }
}
