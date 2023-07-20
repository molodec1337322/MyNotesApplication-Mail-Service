namespace MyNotesApplication_Mail_Service.Services.Interfaces
{
    public interface IService
    {
        string ServiceName { get; }
        void DoWork(string messagePayload);
    }
}
