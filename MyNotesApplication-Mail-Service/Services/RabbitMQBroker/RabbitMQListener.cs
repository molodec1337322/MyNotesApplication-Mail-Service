using MyNotesApplication_Mail_Service.Services.Abstractions;
using MyNotesApplication_Mail_Service.Services.Interfaces;
using MyNotesApplication_Mail_Service.Services.Messages;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MyNotesApplication_Mail_Service.Services.RabbitMQBroker
{
    public class RabbitMQListener
    {
        public delegate void OnMessageRecivedHandler(string message);
        public event OnMessageRecivedHandler OnMessageRecived;

        private readonly IMessageBrokerPersistentConnection _persistentConnection;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQListener> _logger;

        private IModel _channel;

        public RabbitMQListener(IMessageBrokerPersistentConnection persistedConnection, IConfiguration configuration, ILogger<RabbitMQListener> logger)
        {
            _persistentConnection = persistedConnection;
            _configuration = configuration;
            _logger = logger;

            if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();

            _channel = _persistentConnection.CreateModel();

            _channel.QueueDeclare(
                queue: _configuration.GetValue<string>("BrokerNameQueue"),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Listen(Object obj)
        {
            

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_configuration.GetValue<int>("ConnectionsRetry"), retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, ex.Message);
                });

            while ( true )
            {
                if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();

                if (_channel.IsClosed) _channel = _persistentConnection.CreateModel();

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (ch, ea) =>
                {
                    _logger.LogInformation("Reading recived message...");
                    var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Recived message succesfully deserialized!");

                    OnMessageRecived(content);

                    _channel.BasicAck(ea.DeliveryTag, false);
                };

                _channel.BasicConsume(_configuration.GetValue<string>("BrokerNameQueue"), false, consumer);
            }
        }
    }
}
