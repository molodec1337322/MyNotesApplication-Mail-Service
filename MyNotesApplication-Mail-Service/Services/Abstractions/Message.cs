namespace MyNotesApplication_Mail_Service.Services.Abstractions
{
    public abstract class Message
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }

        public Message()
        {
            Id= Guid.NewGuid().ToString();
            Created = DateTime.Now;
        }
    }
}
