using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Attendances.GetTimetableForGroup;

public sealed record GetTimetableForGroupQuery(DateOnly TimetableDate) : IQuery<IReadOnlyList<CourseTimetableForGroupResponse>>;