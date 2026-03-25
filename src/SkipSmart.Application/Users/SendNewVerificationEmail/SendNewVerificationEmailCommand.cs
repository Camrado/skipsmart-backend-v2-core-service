using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Users.SendNewVerificationEmail;

public sealed record SendNewVerificationEmailCommand() : ICommand<Result>;