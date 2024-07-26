using Microsoft.AspNetCore.Authorization;

namespace SkipSmart.Infrastructure.Authorization;

public class EmailVerifiedRequirement : IAuthorizationRequirement {
    public EmailVerifiedRequirement() {
    }
}