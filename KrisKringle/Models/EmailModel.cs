using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PRGPera.Web.Models.Ddr
{
    public class EmailModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public void ENotify(EmailModel model)
        {
            var send = false;
            using (MailMessage mm = new MailMessage())
            {
                mm.Subject = model.Subject;
                mm.Body = model.Body;
                mm.IsBodyHtml = true;
                mm.From = new MailAddress(model.From);
                if (!String.IsNullOrEmpty(model.To))
                {
                    string[] ToId = model.To.Split(',');
                    foreach (string ToEmail in ToId)
                    {
                        mm.To.Add(new MailAddress(ToEmail));
                    }
                    send = true;
                }
                if (!String.IsNullOrEmpty(model.BCC))
                {
                    string[] BccId = model.BCC.Split(',');
                    foreach (string BccEmail in BccId)
                    {
                        mm.Bcc.Add(new MailAddress(BccEmail));
                    }
                    send = true;
                }
                if (!String.IsNullOrEmpty(model.CC))
                {
                    string[] CCId = model.CC.Split(',');
                    foreach (string CCEmail in CCId)
                    {
                        mm.CC.Add(new MailAddress(CCEmail));
                    }
                    send = true;
                }
                
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = model.Host;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(model.From, model.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = model.Port;
                    smtp.Send(mm);
                }
            }
        }
    }
}
