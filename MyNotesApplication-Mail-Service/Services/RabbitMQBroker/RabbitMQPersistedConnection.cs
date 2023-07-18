using MyNotesApplication_Mail_Service.Services.Interfaces;
using RabbitMQ.Client;

namespace MyNotesApplication_Mail_Service.Services.RabbitMQBroker
{
    public class RabbitMQPersistedConnection : IMessageBrokerPersistentConnection
    {
        public bool IsConnected => throw new NotImplementedException();

        public IModel CreateModel()
        {
            throw new NotImplementedException();
        }

        public bool TryConnect()
        {
            throw new NotImplementedException();
        }
    }
}
