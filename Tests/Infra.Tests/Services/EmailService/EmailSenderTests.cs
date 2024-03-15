using Application.Models.Email;
using Infra.Services.EmailService;
using Microsoft.Extensions.Options;


namespace Infra.Tests.Services.EmailService
{
    public class EmailSenderTests
    {
        [Fact]
        public async Task SendEmailAsync_Should_Return_True()
        {
            // Arrange
            var emailSettings = new EmailSettings
            {
                SendGridApiKey = "fake-api-key",
                FromAddress = "from@example.com",
                FromName = "Sender Name"
            };

            var email = new EmailMessage
            {
                To = "to@example.com",
                Subject = "Test Email",
                Body = "This is a test email"
            };

            var mockEmailSettings = Options.Create(emailSettings);
            var emailSender = new EmailSender(mockEmailSettings);

            // Act
            var result = await emailSender.SendEmailAsync(email);

            // Assert
            Assert.True(result);
        }
    }
}
