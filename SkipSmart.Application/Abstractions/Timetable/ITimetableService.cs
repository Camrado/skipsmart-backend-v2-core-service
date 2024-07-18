﻿using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Abstractions.Timetable;

public interface ITimetableService {
    Task<Result<IReadOnlyList<CourseTimetableResponse>>> GetTimetableForDate(Guid groupId, DateOnly timetableDate);
    
    // Cache by GroupId, not GroupId + Subgroup which will multiply everything by two.
    // Return timetable for the whole group and then in the frontend filter by the subgroup. 


    Task<Result<IReadOnlyList<DateOnly>>> GetWorkingDaysForRange(Guid userId, DateOnly startDate, DateOnly endDate);
    // returns all timetables for a user (taking into account the subgroup as well) in a given range.
    // if in some date there's no lesson then the that will not be taken down any way.
    // it will return data only related to existing lessons
}