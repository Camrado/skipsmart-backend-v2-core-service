using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;