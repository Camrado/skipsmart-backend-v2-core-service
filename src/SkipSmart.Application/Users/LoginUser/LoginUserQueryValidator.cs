using FluentValidation;

namespace SkipSmart.Application.Users.LoginUser;

internal sealed class LoginUserQueryValidator : AbstractValidator<LoginUserQuery> {
    public LoginUserQueryValidator() {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();

        RuleFor(c => c.Password).NotEmpty().MinimumLength(8);
    }
}