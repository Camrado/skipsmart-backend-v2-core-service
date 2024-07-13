namespace SkipSmart.Application.Abstractions.Authentication;

public interface IEmailVerificationService {
    Task SendVerificationEmailAsync(Domain.Users.Email recipientEmail, CancellationToken cancellationToken = default);
}