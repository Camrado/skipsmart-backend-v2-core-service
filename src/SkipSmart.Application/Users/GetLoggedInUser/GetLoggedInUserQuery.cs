using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;