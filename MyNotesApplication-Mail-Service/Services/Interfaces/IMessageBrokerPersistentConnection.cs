using Microsoft.AspNetCore.Mvc.ModelBinding;
using RabbitMQ.Client;

namespace MyNotesApplication_Mail_Service.Services.Interfaces
{
    public interface IMessageBrokerPersistentConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}
