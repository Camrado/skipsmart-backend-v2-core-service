namespace SkipSmart.Domain.Groups;

public interface IGroupRepository {
    Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Group>> GetAllAsync(CancellationToken cancellationToken = default);
}