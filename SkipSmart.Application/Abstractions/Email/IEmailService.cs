using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Abstractions.Email;

public interface IEmailService {
    Task<Result> SendEmailAsync(Domain.Users.Email recipientEmail, string subject, string message);
}