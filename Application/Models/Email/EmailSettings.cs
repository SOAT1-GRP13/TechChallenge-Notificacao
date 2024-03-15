namespace Application.Models.Email
{
    public class EmailSettings
    {
        public EmailSettings()
        {
            SendGridApiKey = string.Empty;
            FromAddress = string.Empty;
            FromName = string.Empty;
        }

        public string SendGridApiKey { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
    }
}
