namespace SkipSmart.Domain.Groups;

public interface IGroupRepository {
    Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<Group?> GetByGroupNameAsync(string groupName, CancellationToken cancellationToken = default);
    
    void Add(Group group);
}