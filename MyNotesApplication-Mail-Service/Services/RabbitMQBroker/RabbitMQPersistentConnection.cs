using MyNotesApplication_Mail_Service.Services.Interfaces;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace MyNotesApplication_Mail_Service.Services.RabbitMQBroker
{
    public class RabbitMQPersistentConnection : IMessageBrokerPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQPersistentConnection> _logger;
        private IConnection _connection;
        private bool _disposed;

        private object sync_root = new object();

        public RabbitMQPersistentConnection(
            IConnection connection, 
            IConfiguration configuration, 
            ILogger<RabbitMQPersistentConnection> logger)
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
            lock(sync_root)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_configuration.GetValue<int>("ConnectionsRetry"), retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(ex, ex.ToString());
                    });

                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                if(IsConnected)
                {
                    _connection.CallbackException += OnExceptionCallback;
                    _connection.ConnectionShutdown += OnShutdownConnection;
                    _connection.ConnectionBlocked += OnBlockedConnection;

                    return true;
                }
                _logger.LogCritical("FATAL ERROR: cant create RabbitMQ connection");
                return false;
            }
        }

        private void OnExceptionCallback(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("RabbitMQ connection throw exception.");
            TryConnect();
        }

        private void OnShutdownConnection(object sender, ShutdownEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("RabbitMQ connection shutdown.");
            TryConnect();
        }

        private void OnBlockedConnection(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("RabbitMQ connection blocked.");
            TryConnect();
        }
    }
}
