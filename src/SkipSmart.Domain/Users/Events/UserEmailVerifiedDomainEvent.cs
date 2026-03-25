using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Users.Events;

public sealed record UserEmailVerifiedDomainEvent(Guid Id) : IDomainEvent;