using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PfcSsaNeon.Helper
{
    public class ServicoEmail
    {
        public static void EnviaEmailAsync(string To, string Subject, string Body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress("admssaneon@gmail.com", "administração");
            mail.Subject = Subject;
            mail.Body = Body;
            mail.BodyEncoding = UTF8Encoding.UTF8;
            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = true;
            smtp.Host = "smtp.gmail.com";
            smtp.Port = Convert.ToInt16(587);
            smtp.Timeout = 10000;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("admssaneon@gmail.com", "1q2w3e4r@");
            smtp.Send(mail);
        }
    }
}