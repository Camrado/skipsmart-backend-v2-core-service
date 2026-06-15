namespace SkipSmart.Application.Attendances.GetAttendanceStatistics;

public sealed class CourseAttendanceStatisticsResponse {
    public Guid CourseId { get; set; }
    
    public int AttendedLessonsNumber { get; set; }
    
    public int SkippedLessonsNumber { get; set; }
    
    public int RemainingLessonsNumber { get; set; }
    
    public int RemainingSkipsNumber { get; set; }
    
    public int TotalLessonsNumber { get; set; }
}