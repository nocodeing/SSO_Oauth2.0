using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Common.IService;
using CommonTools;
using Model.Const;

namespace Common.Service
{
    public class EmailService : IEmailService
    {
        public bool Send(string email, string content)
        {
            if (RequestHelper.GetServerIp() != "203.171.233.12")
            {
                return true;
            }
            try
            {
                var e = new MailMessage(EmailConst.FromName, email, EmailConst.Subject, content)
                {
                    Priority = MailPriority.High,
                    IsBodyHtml = EmailConst.MailBodyHtml,
                    BodyEncoding = Encoding.UTF8
                };
                var sc = new SmtpClient(EmailConst.SmtpServer, EmailConst.ServerPort)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 100000,
                    Credentials = new System.Net.NetworkCredential(EmailConst.FromName, EmailConst.EmailPwd)
                };
                sc.Send(e);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Send(List<string> email, string content)
        {
            if (RequestHelper.GetServerIp() != "203.171.233.12")
            {
                return true;
            }
            try
            {
                var e = new MailMessage()
                {
                    From = new MailAddress(EmailConst.FromName),
                    Subject = EmailConst.Subject,
                    Body = content,
                    Priority = MailPriority.High,
                    IsBodyHtml = EmailConst.MailBodyHtml,
                    BodyEncoding = Encoding.UTF8
                };
                foreach (string t in email)
                {
                    e.To.Add(t);
                }
                var sc = new SmtpClient(EmailConst.SmtpServer, EmailConst.ServerPort)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 100000,
                    Credentials = new System.Net.NetworkCredential(EmailConst.FromName, EmailConst.EmailPwd)
                };

                sc.Send(e);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
