using MyNotesApplication_Mail_Service.Services.Interfaces;
using MyNotesApplication_Mail_Service.Services.Messages;

namespace MyNotesApplication_Mail_Service.Services.RabbitMQBroker
{
    public class RabbitMQSubscriptionManager : IServiceSubscriptionManager
    {
        private List<IService> _services;

        public RabbitMQSubscriptionManager() 
        { 
            _services = new List<IService>();
        }

        public void AddSubscribtion(IService service)
        {
            _services.Add(service);
        }

        public void OnMessageRecived(MessageBase messageBase)
        {
            var service = _services.FirstOrDefault(s => s.ServiceName == messageBase.ServiceName);
            if (service == null) throw new NullReferenceException("No specified service found");

            service.DoWork(messageBase.JSONPayload);
        }
    }
}
