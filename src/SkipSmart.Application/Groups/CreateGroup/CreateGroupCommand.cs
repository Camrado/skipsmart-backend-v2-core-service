using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Groups.CreateGroup;

public record CreateGroupCommand(string GroupName, int EdupageClassId) : ICommand<Guid>;
