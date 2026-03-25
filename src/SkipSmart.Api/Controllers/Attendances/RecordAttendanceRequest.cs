namespace SkipSmart.Api.Controllers.Attendances;

public record RecordAttendanceRequest(
    DateOnly AttendanceDate, 
    bool IsDateMarked, 
    Guid CourseId,
    bool HasAttended,
    int Period);