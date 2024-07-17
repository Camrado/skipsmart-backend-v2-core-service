using Microsoft.EntityFrameworkCore;
using SkipSmart.Domain.Courses;

namespace SkipSmart.Infrastructure.Repositories;

internal sealed class CourseRepository : Repository<Course>, ICourseRepository {
    public CourseRepository(ApplicationDbContext dbContext) : base(dbContext) {
    }

    public async Task<IReadOnlyCollection<Course>> GetAllByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default) {
        return await DbContext
            .Set<Course>()
            .Where(c => c.GroupId == groupId)
            .ToListAsync(cancellationToken);
    }
}