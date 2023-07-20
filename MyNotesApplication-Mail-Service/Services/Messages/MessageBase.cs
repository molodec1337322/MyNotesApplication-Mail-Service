namespace MyNotesApplication_Mail_Service.Services.Messages
{
    public class MessageBase
    {
        public string Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string JSONPayload { get; set; }
        public string ServiceName { get; set; }
    }
}
