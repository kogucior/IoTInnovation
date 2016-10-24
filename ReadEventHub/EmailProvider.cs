using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ReadEventHub
{
    public class EmailProvider
    {
        readonly MailAddress fromAddress = new MailAddress("innovationObjectivity2016@gmail.com", "innovationObjectivity2016@gmail.com");
        readonly MailAddress toAddress = new MailAddress("kamil.kociuga@interia.pl", "kamil.kociuga@interia.pl");
        readonly string subject = "Alert. Temperature greater than acceptable threshold.";

        public async Task SendEmailAlert(string messageBody)
        {
            var body = GetBodyFromMessage(messageBody);

            var smtp = Smtp;

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body

            })
            {
                await smtp.SendMailAsync(message);
            }
        }

        private static string GetBodyFromMessage(string messageBody)
        {
            dynamic tmp = JObject.Parse(messageBody);
            var s = new StringBuilder();
            s.AppendLine("The device: " + tmp.deviceId);
            s.AppendLine("DateTime.UtcNow is: " + DateTime.UtcNow);
            s.AppendLine("Telemetry read at: " + tmp.messageDateTime);
            s.AppendLine("Thermometer shows: " + tmp.temperature + "*C");
            return s.ToString();
        }

        private SmtpClient Smtp => new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, "_PL<mko0")
        };
    }
}
