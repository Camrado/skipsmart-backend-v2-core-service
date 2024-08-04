using Microsoft.EntityFrameworkCore;
using SkipSmart.Domain.MarkedDates;

namespace SkipSmart.Infrastructure.Repositories;

internal sealed class MarkedDateRepository : Repository<MarkedDate>, IMarkedDateRepository {
    public MarkedDateRepository(ApplicationDbContext dbContext) : base(dbContext) {
    }

    public async Task<MarkedDate?> GetByDetailsAsync(DateOnly recordedDate, Guid userId, CancellationToken cancellationToken = default) {
        return await DbContext
            .Set<MarkedDate>()
            .FirstOrDefaultAsync(md => md.UserId == userId && md.RecordedDate == recordedDate, cancellationToken);
    }
    
    public void DeleteByUserId(Guid userId) {
        var markedDatesToRemove = DbContext
            .Set<MarkedDate>()
            .Where(md => md.UserId == userId)
            .ToList();
        
        DbContext.RemoveRange(markedDatesToRemove);
    }
}