﻿namespace SkipSmart.Domain.Courses;

public interface ICourseRepository {
    Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Course>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Course>> GetAllByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);
}