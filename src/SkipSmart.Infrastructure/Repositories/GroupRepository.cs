using Microsoft.EntityFrameworkCore;
using SkipSmart.Domain.Groups;

namespace SkipSmart.Infrastructure.Repositories;

internal sealed class GroupRepository : Repository<Group>, IGroupRepository {
    public GroupRepository(ApplicationDbContext dbContext) : base(dbContext) {
    }

    public async Task<Group?> GetByGroupNameAsync(string groupName, CancellationToken cancellationToken = default) {
        return await DbContext
            .Set<Group>()
            .FirstOrDefaultAsync(g => g.GroupName == new GroupName(groupName), cancellationToken);
    }
}