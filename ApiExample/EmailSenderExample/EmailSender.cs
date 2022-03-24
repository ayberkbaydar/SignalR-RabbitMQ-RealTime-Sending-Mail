using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderExample
{
    static class EmailSender
    {
        public static void Send(string to, string message)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            NetworkCredential credential = new NetworkCredential("senderMailAddress", "password");
            smtpClient.Credentials = credential;

            MailAddress gonderen = new MailAddress("senderMailAddress", "Name Surname");
            MailAddress alici = new MailAddress(to);

            MailMessage mail = new MailMessage(gonderen, alici);
            mail.Subject = "Subject";
            mail.Body = message;

            smtpClient.Send(mail);
        }
    }
}
