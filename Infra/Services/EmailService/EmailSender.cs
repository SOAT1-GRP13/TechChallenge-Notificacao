using Application.Common.Interfaces.Services;
using Application.Models.Email;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;


namespace Infra.Services.EmailService
{
    public class EmailSender : IEmailSender
    {
        public EmailSettings _emailSettings { get; }
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        //public async Task<bool> SendEmailAsync(EmailMessage email)
        //{
        //    var client = new SendGridClient(_emailSettings.SendGridApiKey);
        //    var to = new EmailAddress(email.To);
        //    var from = new EmailAddress
        //    {
        //        Email = _emailSettings.FromAddress,
        //        Name = _emailSettings.FromName
        //    };

        //    var message = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.Body);
        //    var response = await client.SendEmailAsync(message);

        //    return response.IsSuccessStatusCode;
        //}

        #pragma warning disable CS1998
        public async Task<bool> SendEmailAsync(EmailMessage email)
        {        
            return true;
        }
        #pragma warning restore CS1998
    }
}
