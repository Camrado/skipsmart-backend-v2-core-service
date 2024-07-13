namespace SkipSmart.Domain.Attendances;

public interface IAttendanceRepository {
    Task<Attendance?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Attendance>> GetByDateAsync(DateOnly date, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Attendance>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    void Add(Attendance attendance);
    
    void Update(Guid id, Attendance newAttendance);
}