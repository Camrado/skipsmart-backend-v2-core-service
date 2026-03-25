using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.Shared;

namespace SkipSmart.Application.Users.VerifyEmail;

public sealed record VerifyEmailCommand(int EmailVerificationCode) : ICommand<AccessTokenResponse>;