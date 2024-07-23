using SkipSmart.Application.Abstractions.Email;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Infrastructure.Email;

internal sealed class EmailService : IEmailService {
    public Task<Result> SendEmailAsync(Domain.Users.Email recipientEmail, string subject, string message) {
        throw new NotImplementedException();
    }
}