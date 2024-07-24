using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.MarkedDates;

public class MarkedDate : Entity {
    public DateOnly RecordedDate { get; private set; }
    
    public Guid UserId { get; private set; }
    
    private MarkedDate(Guid id, DateOnly recordedDate, Guid userId) : base(id) {
        RecordedDate = recordedDate;
        UserId = userId;
    }
    
    private MarkedDate() {
    }
    
    public static MarkedDate Create(DateOnly recordedDate, Guid userId) {
        return new(Guid.NewGuid(), recordedDate, userId);
    }
}