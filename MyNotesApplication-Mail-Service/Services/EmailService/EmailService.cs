using MailKit.Net.Smtp;
using MimeKit;
using MyNotesApplication_Mail_Service.Services.EmailService.Message;
using MyNotesApplication_Mail_Service.Services.Interfaces;
using MyNotesApplication_Mail_Service.Services.Messages;
using MyNotesApplication_Mail_Service.Services.RabbitMQBroker;
using System.Text.Json;

namespace MyNotesApplication_Mail_Service.Services.EmailService
{
    public class EmailService : IService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private const string KEY = "EMAIL_SERVICE";

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string ServiceName => KEY;

        public void DoWork(string messagePayload)
        {
            SendEmailAsync(messagePayload);
        }

        private async Task SendEmailAsync(string messagePayload)
        {
            var content = JsonSerializer.Deserialize<EmailRabbitMQMessage>(messagePayload);
            _logger.LogInformation($"Sending Mail to {content.Email}...");

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("My Note Application", _configuration.GetValue<string>("HostEmail")));
            emailMessage.To.Add(new MailboxAddress("My Note Application", content.Email));
            emailMessage.Subject = content.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = content.Body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(_configuration.GetValue<string>("HostEmail"), _configuration.GetValue<string>("HostPassword"));
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
