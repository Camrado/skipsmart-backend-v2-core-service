using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Courses;

public class Course : Entity {
    public string CourseName { get; private set; }
    public Semester Semester { get; private set; }
    
    public decimal Hours { get; private set; }
    
    public Guid GroupId { get; private set; }
    
    public Course(Guid id, string courseName, Semester semester, Guid groupId, decimal hours) : base(id) {
        CourseName = courseName;
        Semester = semester;
        GroupId = groupId;
        Hours = hours;
    }

    private Course() {
    }
}