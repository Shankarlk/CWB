using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace CWB.App.Services.EmailServices
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_config["EmailSettings:SenderName"], _config["EmailSettings:SenderEmail"]));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(_config["EmailSettings:SmtpServer"],
                                      int.Parse(_config["EmailSettings:Port"]),
                                      SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

    }
}
