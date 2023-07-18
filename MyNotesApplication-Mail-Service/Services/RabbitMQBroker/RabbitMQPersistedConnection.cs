using MyNotesApplication_Mail_Service.Services.Interfaces;
using RabbitMQ.Client;

namespace MyNotesApplication_Mail_Service.Services.RabbitMQBroker
{
    public class RabbitMQPersistedConnection : IMessageBrokerPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQPersistedConnection> _logger;

        private bool _disposed;

        public RabbitMQPersistedConnection(
            IConnection connection, 
            IConfiguration configuration, 
            ILogger<RabbitMQPersistedConnection> logger)
        {
            _configuration = configuration;
            _connectionFactory = new ConnectionFactory() { HostName = configuration.GetValue<string>("MessageBrokerHost") };
            _logger = logger;
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

        public bool TryConnect()
        {
            throw new NotImplementedException();
        }
    }
}
