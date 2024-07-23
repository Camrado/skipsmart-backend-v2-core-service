using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Abstractions.Authentication;

public interface IEmailVerificationService {
    Task<Result> SendVerificationEmailAsync(User user, CancellationToken cancellationToken = default);
}