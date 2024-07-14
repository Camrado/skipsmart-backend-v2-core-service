namespace SkipSmart.Domain.MarkedDates;

public interface IMarkedDateRepository {
    Task<MarkedDate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MarkedDate?> GetByDetailsAsync(DateOnly recordedDate, Guid userId, CancellationToken cancellationToken = default);
    
    void Add(MarkedDate user);
}