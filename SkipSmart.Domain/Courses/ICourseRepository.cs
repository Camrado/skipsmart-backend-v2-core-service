namespace SkipSmart.Domain.Courses;

public interface ICourseRepository {
    Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<Course?> GetByCourseNameAsync(string courseName, Guid groupId, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Course>> GetAllByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);
    
    void Add(Course course);
}