using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Attendances.RecordAttendance;

public sealed record RecordAttendanceCommand(
    DateOnly AttendanceDate, 
    bool IsDateMarked, 
    Guid CourseId,
    bool HasAttended,
    int Period) : ICommand<Result>;