using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.CourseHours;

public class CourseHour : Entity {
    public decimal Hours { get; private set; }
    
    public Guid CourseId { get; private set; }
    
    private CourseHour(Guid id, decimal hours, Guid courseId) : base(id) {
        Hours = hours;
        CourseId = courseId;
    }
    
    private CourseHour() {
    }
}