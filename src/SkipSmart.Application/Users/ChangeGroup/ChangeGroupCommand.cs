using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.Shared;

namespace SkipSmart.Application.Users.ChangeGroup;

public sealed record ChangeGroupCommand(Guid NewGroupId) : ICommand<AccessTokenResponse>;