using MyNotesApplication_Mail_Service.Services.Abstractions;
using MyNotesApplication_Mail_Service.Services.Interfaces;
using MyNotesApplication_Mail_Service.Services.Messages;
using MyNotesApplication_Mail_Service.Services.RabbitMQBroker;

namespace MyNotesApplication_Mail_Service.Services
{
    public class EmailSender
    {
        private readonly RabbitMQListener _listener;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(RabbitMQListener listener, ILogger<EmailSender> logger)
        {
            _listener = listener;
            _listener.OnMessageRecived += SendEmail;
            _logger = logger;
        }

        private void SendEmail(string email) 
        {
            _logger.LogInformation("Sending Mail...");
        }
    }
}
