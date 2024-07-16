namespace SkipSmart.Application.Abstractions.Timetable;

public class CourseTimetableResponse {
    public int Period { get; set; }
    
    public DateOnly TimetableDate { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public string CourseName { get; set; }
    
    public int Subgroup { get; set; }
}