using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.Shared;

namespace SkipSmart.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    Guid GroupId) : ICommand<AccessTokenResponse>;