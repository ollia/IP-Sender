namespace IPSender
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Mail;

    public class MailSender
    {
        public void SendMail(string message)
        {
            try
            {
                var smtp = ConfigurationManager.AppSettings.Get("SmtpServer");
                var port = int.Parse(ConfigurationManager.AppSettings.Get("SmtpPort"));
                var sender = ConfigurationManager.AppSettings.Get("MailSender");
                var receiver = ConfigurationManager.AppSettings.Get("MailReceiver");
                var senderPassword = ConfigurationManager.AppSettings.Get("SenderPassword");
                var mail = new MailMessage(sender, receiver);
                var client = new SmtpClient
                {
                    Port = port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = smtp
                };

                client.Credentials = new NetworkCredential(sender, senderPassword);
                client.EnableSsl = true;

                mail.Subject = "IP-address change";
                mail.Body = message;
                Trace.Write(DateTime.Now + ":" + message);
                Trace.Write(Environment.NewLine);
                client.Send(mail);
            }
            catch (Exception e)
            {
                Trace.Write(DateTime.Now.ToString("s") + ":" + "Mail sender exception: " + e);
                Trace.Write(Environment.NewLine);
            }
        }
    }
}
