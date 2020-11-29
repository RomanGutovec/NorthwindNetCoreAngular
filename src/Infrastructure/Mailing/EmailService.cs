using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Mailing.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Mailing
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        //public void Send(string from, string to, string subject, string html)
        //{
        //    // create message
        //    var email = new MimeMessage();
        //    email.From.Add(MailboxAddress.Parse(from));
        //    email.To.Add(MailboxAddress.Parse(to));
        //    email.Subject = subject;
        //    email.Body = new TextPart(TextFormat.Html) { Text = html };

        //    // send email
        //    using var smtp = new SmtpClient();
        //    smtp.Connect(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
        //    smtp.Authenticate(_smtpSettings.UserName, _smtpSettings.Password);
        //    smtp.Send(email);
        //    smtp.Disconnect(true);
        //}

        public void Send(string email, string subject, string html)
        {
            // create message
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            emailMessage.To.Add(MailboxAddress.Parse(email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpSettings.UserName, _smtpSettings.Password);
            smtp.Send(emailMessage);
            smtp.Disconnect(true);
        }
    }
}