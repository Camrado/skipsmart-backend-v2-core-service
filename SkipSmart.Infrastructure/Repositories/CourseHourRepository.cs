using Microsoft.EntityFrameworkCore;
using SkipSmart.Domain.CourseHours;

namespace SkipSmart.Infrastructure.Repositories;

internal sealed class CourseHourRepository : Repository<CourseHour>, ICourseHourRepository {
    public CourseHourRepository(ApplicationDbContext dbContext) : base(dbContext) {
    }

    public async Task<CourseHour?> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default) {
        return await DbContext
            .Set<CourseHour>()
            .FirstOrDefaultAsync(ch => ch.CourseId == courseId, cancellationToken);
    }
}