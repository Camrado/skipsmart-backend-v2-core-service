using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Groups.DeleteGroup;

public record DeleteGroupCommand(Guid GroupId) : ICommand;
