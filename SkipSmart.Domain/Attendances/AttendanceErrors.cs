using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Attendances;

public static class AttendanceErrors {
    public static Error NotFound = new(
        "Attendance.NotFound",
        "The attendance with the specified identifier was not found");
}