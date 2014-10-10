namespace IPSender
{
    using System;
    using System.Configuration;
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

                bool enableSsl;
                bool.TryParse(ConfigurationManager.AppSettings.Get("EnableSsl"), out enableSsl);

                var client = new SmtpClient
                {
                    Port = port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = smtp,
                    Credentials = new NetworkCredential(sender, senderPassword),
                    EnableSsl = enableSsl
                };

                var mail = new MailMessage(sender, receiver) {Subject = "IP-address change", Body = message};

                TraceLogger.Write("Sending mail: " + message);                
                client.Send(mail);
            }
            catch (Exception e)
            {
                TraceLogger.Write("Mail sender exception: " + e);
            }
        }
    }
}
