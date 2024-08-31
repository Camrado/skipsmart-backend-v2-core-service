namespace SkipSmart.Application.Attendances.GetTimetableForGroup;

public class CourseTimetableForGroupResponse {
    public int Period { get; set; }
    
    public string CourseName { get; set; }
    
    public Guid CourseId { get; set; }
    
    public int FacultySubgroup { get; set; }
    
    public int LanguageSubgroup { get; set; }
    
    public string Teacher { get; set; }
}