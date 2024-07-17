using Microsoft.EntityFrameworkCore;
using SkipSmart.Domain.Users;

namespace SkipSmart.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User>, IUserRepository {
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext) {
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) {
        return await DbContext.Set<User>().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}