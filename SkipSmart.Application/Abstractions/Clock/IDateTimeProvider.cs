namespace SkipSmart.Application.Abstractions.Clock;

public interface IDateTimeProvider {
    DateTime UtcNow { get; }
    
    DateOnly TodayInBaku { get; }
    
    DateTime DateInBaku { get; }
    
    DateOnly SemesterStartDate { get; }
}