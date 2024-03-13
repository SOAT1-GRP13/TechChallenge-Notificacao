using Application.Models.Email;


namespace Application.Common.Interfaces.Services
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailMessage email);
    }
}
