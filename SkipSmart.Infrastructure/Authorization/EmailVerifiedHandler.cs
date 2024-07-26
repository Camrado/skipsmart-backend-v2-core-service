using Microsoft.AspNetCore.Authorization;
using SkipSmart.Application.Abstractions.Authentication;

namespace SkipSmart.Infrastructure.Authorization;

public class EmailVerifiedHandler : AuthorizationHandler<EmailVerifiedRequirement> {
    private readonly IUserContext _userContext;

    public EmailVerifiedHandler(IUserContext userContext) {
        _userContext = userContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailVerifiedRequirement requirement) {
        var isEmailVerified = _userContext.IsEmailVerified;

        if (isEmailVerified) {
            context.Succeed(requirement);
        } else {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}