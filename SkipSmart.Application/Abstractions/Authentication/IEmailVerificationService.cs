using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Abstractions.Authentication;

public interface IEmailVerificationService {
    Task<Result> SendVerificationEmailAsync(Domain.Users.Email recipientEmail, CancellationToken cancellationToken = default);
}