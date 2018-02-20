using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Utilities
{
    public static class Email
    {
        public static void sendEmail(string to,string subject,string message)
        {
            var fromAddress = new MailAddress("support@sdtrconsulting.com", "SDTR Consulting");
            var toAddress = new MailAddress(to);
             string SubjectM = subject;
             string body = message;
          

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("test42850@gmail.com", "MyTest@123")
            };
            using (var msg = new MailMessage(fromAddress, toAddress)
            {
                Subject = SubjectM,
                Body = body
            })
            {
                msg.IsBodyHtml = true;
                smtp.Send(msg);
            }

        }


    }
}
