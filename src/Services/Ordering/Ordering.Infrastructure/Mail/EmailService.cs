using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettings EmailSettings { get; }

        public ILogger<EmailService> Logger { get; }


        public EmailService(EmailSettings emailSettings, ILogger<EmailService> logger)
        {
            EmailSettings = emailSettings ?? throw new ArgumentNullException(nameof(emailSettings));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SendEmail(Email email)
        {
            Logger.LogInformation($"Email sent to {email.To}");
            return true;
        }
    }
}
