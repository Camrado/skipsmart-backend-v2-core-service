namespace SkipSmart.Domain.CourseHours;

public interface ICourseHourRepository {
    Task<CourseHour?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<CourseHour?> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default);
    
    void Add(CourseHour courseHour);
}