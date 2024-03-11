namespace Application.Models.Email
{
    public class EmailSettings
    {
        public string SendGridApiKey { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
    }
}
