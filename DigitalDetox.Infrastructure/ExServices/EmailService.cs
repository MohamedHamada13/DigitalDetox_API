using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace DigitalDetox.Infrastructure.ExServices
{
    public static class EmailService
    {
        public static string SendEmail(string emailTo, string subject, string body)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;

                client.Credentials = new NetworkCredential("digitaldetox259@gmail.com", "psmi tqvd jwkf cddi");

                client.Send("digitaldetox259@gmail.com", emailTo, subject, body);
                return string.Empty;
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }

        }
    }
}
