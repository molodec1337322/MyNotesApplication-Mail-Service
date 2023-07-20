using MyNotesApplication_Mail_Service.Services.EmailService;
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
    public class RabbitMQListener : BackgroundService
    {
        private readonly IMessageBrokerPersistentConnection _persistentConnection;
        private readonly IServiceSubscriptionManager _serviceSubscriptionManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQListener> _logger;

        private IModel _channel;

        public RabbitMQListener(
            IMessageBrokerPersistentConnection persistedConnection, 
            IServiceSubscriptionManager serviceSubscriptionManager, 
            IConfiguration configuration, ILogger<RabbitMQListener> logger,
            EmailService.EmailService emailSender
            )
        {
            _persistentConnection = persistedConnection;
            _configuration = configuration;
            _logger = logger;

            _serviceSubscriptionManager = serviceSubscriptionManager;
            _serviceSubscriptionManager.AddSubscribtion(emailSender);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();

            if (_channel == null) _channel = _persistentConnection.CreateModel();

            _channel.QueueDeclare(
                    queue: _configuration.GetValue<string>("BrokerMessageQueue"),
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                _logger.LogInformation("Reading recived message...");
                var messageBase = JsonSerializer.Deserialize<MessageBase>(Encoding.UTF8.GetString(ea.Body.ToArray()));
                _logger.LogInformation("Recived message succesfully deserialized!");

                _serviceSubscriptionManager.OnMessageRecived(messageBase);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_configuration.GetValue<string>("BrokerMessageQueue"), false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _persistentConnection.Dispose();
            base.Dispose();
        }
    }
}
