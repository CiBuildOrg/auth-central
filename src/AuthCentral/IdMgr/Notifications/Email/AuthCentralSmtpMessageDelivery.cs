using System;
using System.Text;
using System.Net.Mime;
using System.Net.Mail;

using Newtonsoft.Json;
using BrockAllen.MembershipReboot;

namespace Fsw.Enterprise.AuthCentral.IdMgr.Notifications.Email
{
    public class AuthCentralSmtpMessageDelivery : IMessageDelivery
    {
        public enum MsgBodyTypes
        {
            PlainText = 1,
            Html = 2, 
            MultipartAlternativeAsJson = 3
        }

        public EnvConfig Config { get; set; }
        public MsgBodyTypes MsgBodyType { get; set; }
        public int SmtpTimeout { get; set; }

        public AuthCentralSmtpMessageDelivery(EnvConfig config, MsgBodyTypes bodyType, int smtpTimeout = 5000)
        {
            this.Config = config;
            this.MsgBodyType = bodyType;
            this.SmtpTimeout = smtpTimeout;
        }

        public void Send(Message msg)
        {
            Tracing.Information("[SmtpMessageDelivery.Send] sending mail to " + msg.To);

            if (String.IsNullOrWhiteSpace(msg.From))
            {
                msg.From = Config.Smtp.From;
            }

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = Config.Smtp.Host;
                smtp.Timeout = SmtpTimeout;

                try
                {
                    MailMessage mailMessage;

                    switch (MsgBodyType)
                    {
                        case MsgBodyTypes.Html:
                        {
                            mailMessage = new MailMessage(msg.From, msg.To, msg.Subject, msg.Body);
                            mailMessage.IsBodyHtml = true;
                            break;
                        }
                        case MsgBodyTypes.MultipartAlternativeAsJson:
                        {
                            MultipartMessageBody multipart = JsonConvert.DeserializeObject<MultipartMessageBody>(msg.Body);

                            mailMessage = new MailMessage(msg.From, msg.To, msg.Subject, multipart.PlainText);
                            mailMessage.BodyEncoding = Encoding.UTF8;
                            mailMessage.SubjectEncoding = Encoding.UTF8;

                            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(multipart.Html, null, MediaTypeNames.Text.Html);
                            mailMessage.AlternateViews.Add(htmlView);
                            break;
                        }
                        default:
                        {
                            mailMessage = new MailMessage(msg.From, msg.To, msg.Subject, msg.Body);
                            break;
                        }
                    }

                   smtp.Send(mailMessage);
                }
                catch (SmtpException e)
                {
                    Tracing.Error("[SmtpMessageDelivery.Send] SmtpException: " + e.Message);
                }
                catch (Exception e)
                {
                    Tracing.Error("[SmtpMessageDelivery.Send] Exception: " + e.Message);
                }
            }
        }

    }
}
