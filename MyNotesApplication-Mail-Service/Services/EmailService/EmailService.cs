using MyNotesApplication_Mail_Service.Services.EmailService.Message;
using MyNotesApplication_Mail_Service.Services.Interfaces;
using MyNotesApplication_Mail_Service.Services.Messages;
using MyNotesApplication_Mail_Service.Services.RabbitMQBroker;
using System.Text.Json;

namespace MyNotesApplication_Mail_Service.Services.EmailService
{
    public class EmailService : IService
    {
        private readonly ILogger<EmailService> _logger;
        private const string KEY = "EMAIL_SERVICE";

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public string ServiceName => KEY;

        public void DoWork(string messagePayload)
        {
            SendEmail(messagePayload);
        }

        private void SendEmail(string messagePayload)
        {
            var content = JsonSerializer.Deserialize<EmailRabbitMQMessage>(messagePayload);
            _logger.LogInformation($"Sending Mail to {content.Email}...");
        }
    }
}
