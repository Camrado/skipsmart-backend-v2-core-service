namespace SkipSmart.Application.Abstractions.Timetable;

public class CourseTimetableResponse {
    public int Period { get; set; }
    
    public DateOnly TimetableDate { get; set; }
    
    public string CourseName { get; set; }
    
    public int Subgroup { get; set; }
}