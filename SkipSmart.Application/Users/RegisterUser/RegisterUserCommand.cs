using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.Shared;

namespace SkipSmart.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    int LanguageSubgroup,
    int FacultySubgroup,
    string Password,
    Guid GroupId,
    Guid? UserId) : ICommand<AccessTokenResponse>;