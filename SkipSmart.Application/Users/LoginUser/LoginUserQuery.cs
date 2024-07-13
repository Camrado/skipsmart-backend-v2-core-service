using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.Shared;

namespace SkipSmart.Application.Users.LoginUser;

public sealed record LoginUserQuery(string Email, string Password) : IQuery<AccessTokenResponse>;