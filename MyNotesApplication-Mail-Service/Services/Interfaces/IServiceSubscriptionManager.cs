using MyNotesApplication_Mail_Service.Services.Messages;

namespace MyNotesApplication_Mail_Service.Services.Interfaces
{
    public interface IServiceSubscriptionManager
    {
        void AddSubscribtion(IService service);
        void OnMessageRecived(MessageBase messageBase);
    }
}
