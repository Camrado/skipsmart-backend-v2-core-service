using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Groups.GetAllGroups;

public sealed record GetAllGroupsQuery() : IQuery<IReadOnlyList<GroupResponse>>;