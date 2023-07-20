namespace MyNotesApplication_Mail_Service.Services.EmailService.Message
{
    public class EmailRabbitMQMessage
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailRabbitMQMessage(string Email, string Subject, string Body)
        {
            this.Email = Email;
            this.Subject = Subject;
            this.Body = Body;
        }
    }
}
