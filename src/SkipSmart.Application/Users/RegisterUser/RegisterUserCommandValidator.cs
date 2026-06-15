using FluentValidation;

namespace SkipSmart.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand> {
    public RegisterUserCommandValidator() {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();

        RuleFor(c => c.FirstName).NotEmpty();

        RuleFor(c => c.LastName).NotEmpty();

        RuleFor(c => c.Password).NotEmpty().MinimumLength(8);

        RuleFor(c => c.GroupId).NotEmpty();
    }
}