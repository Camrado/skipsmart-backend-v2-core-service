using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Groups.UpdateGroup;

public record UpdateGroupCommand(Guid GroupId, string GroupName, int EdupageClassId) : ICommand;
