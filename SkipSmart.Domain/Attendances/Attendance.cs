using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Attendances;

public class Attendance : Entity {
    public Guid CourseId { get; private set; }
    public Guid UserId { get; private set; }
    
    public bool HasAttended { get; set; }
    public DateOnly AttendanceDate { get; private set; }
    public Period Period { get; private set; }
    
    private Attendance(Guid id, Guid courseId, Guid userId, bool hasAttended, DateOnly attendanceDate, Period period) : base(id) {
        CourseId = courseId;
        UserId = userId;
        HasAttended = hasAttended;
        AttendanceDate = attendanceDate;
        Period = period;
    }
    
    public static Attendance Create(Guid courseId, Guid userId, bool hasAttended, DateOnly attendanceDate, Period period) {
        return new(Guid.NewGuid(), courseId, userId, hasAttended, attendanceDate, period);
    }
}