using FluentValidation;
using SkipSmart.Application.Users.RegisterUser;

namespace SkipSmart.Application.Users.ChangeGroup;

internal sealed class ChangeGroupCommandValidator : AbstractValidator<ChangeGroupCommand> {
    public ChangeGroupCommandValidator() {
        RuleFor(c => c.NewGroupName).NotEmpty();
    }
}