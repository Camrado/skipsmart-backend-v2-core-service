namespace SkipSmart.Application.Abstractions.Clock;

public interface IDateTimeProvider {
    DateTime UtcNow { get; }
    
    DateOnly TodayInBaku { get; }
    
    DateTime DateTimeInBaku { get; }
    
    DateOnly SemesterStartDate { get; }
    
    DateOnly FirstSemesterStartDate { get; }
    
    DateOnly SecondSemesterStartDate { get; }
}