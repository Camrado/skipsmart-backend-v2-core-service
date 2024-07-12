using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Attendances;

public class Attendance : Entity {
    public Guid CourseId { get; private set; }
    public Guid UserId { get; private set; }
    
    public bool HasAttended { get; private set; }
    public DateOnly AttendanceDate { get; private set; }
    public Period Period { get; private set; }
    
    public Attendance(Guid id, Guid courseId, Guid userId, bool hasAttended, DateOnly attendanceDate, Period period) : base(id) {
        CourseId = courseId;
        UserId = userId;
        HasAttended = hasAttended;
        AttendanceDate = attendanceDate;
        Period = period;
    }
}