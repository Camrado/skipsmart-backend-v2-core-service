using Microsoft.EntityFrameworkCore;
using SkipSmart.Domain.Attendances;

namespace SkipSmart.Infrastructure.Repositories;

internal sealed class AttendanceRepository : Repository<Attendance>, IAttendanceRepository {
    public AttendanceRepository(ApplicationDbContext dbContext) : base(dbContext) {
    }

    public async Task<Attendance?> GetByDetailsAsync(DateOnly attendanceDate, Period period, Guid userId,
        CancellationToken cancellationToken = default) 
    {
        return await DbContext
            .Set<Attendance>()
            .FirstOrDefaultAsync(a => 
                    a.AttendanceDate == attendanceDate && 
                    a.Period == period 
                    && a.UserId == userId,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<Attendance>> GetByDateAsync(DateOnly attendanceDate, Guid userId, CancellationToken cancellationToken = default) {
        return await DbContext
            .Set<Attendance>()
            .Where(a => a.AttendanceDate == attendanceDate && a.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Attendance>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) {
        return await DbContext
            .Set<Attendance>()
            .Where(a => a.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public void DeleteByUserId(Guid userId, CancellationToken cancellationToken = default) {
        var attendancesToRemove = DbContext
            .Set<Attendance>()
            .Where(a => a.UserId == userId)
            .ToListAsync(cancellationToken);
        
        DbContext.RemoveRange(attendancesToRemove);
    }
}