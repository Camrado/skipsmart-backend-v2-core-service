namespace SkipSmart.Domain.CourseHours;

public interface ICourseHourRepository {
    Task<CourseHour?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<CourseHour?> GetByCourseIdAsync(Guid id, CancellationToken cancellationToken = default);
}