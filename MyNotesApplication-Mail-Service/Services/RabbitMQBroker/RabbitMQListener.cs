using MyNotesApplication_Mail_Service.Services.Interfaces;

namespace MyNotesApplication_Mail_Service.Services.RabbitMQBroker
{
    public class RabbitMQListener : BackgroundService
    {

        private IMessageBrokerPersistentConnection _persistedConnection;
        private ILogger<RabbitMQListener> _logger;

        public RabbitMQListener(IMessageBrokerPersistentConnection persistedConnection, ILogger<RabbitMQListener> logger)
        {
            _persistedConnection = persistedConnection;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
