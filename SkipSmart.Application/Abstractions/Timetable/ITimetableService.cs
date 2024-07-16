using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Abstractions.Timetable;

public interface ITimetableService {
    Task<Result<IReadOnlyList<CourseTimetableResponse>>> GetTimetableForDate(Guid groupId, DateOnly timetableDate);
    
    // Cache by GroupId, not GroupId + Subgroup which will multiply everything by two.
    // Return timetable for the whole group and then in the frontend filter by the subgroup. 
}