namespace SkipSmart.Application.Courses.GetCoursesForGroup;

public class CourseResponse {
    public Guid Id { get; set; }
    
    public string CourseName { get; set; }
    
    public int Semester { get; set; }
}