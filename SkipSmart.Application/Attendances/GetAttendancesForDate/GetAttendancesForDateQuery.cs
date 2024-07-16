using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Attendances.GetAttendancesForDate;

public sealed record GetAttendancesForDateQuery(DateOnly AttendanceDate) : IQuery<IReadOnlyList<AttendanceForDateResponse>>;