using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Abstractions.Timetable;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Attendances;

namespace SkipSmart.Application.Attendances.GetTimetableForGroup;

internal sealed class GetTimetableForGroupQueryHandler : IQueryHandler<GetTimetableForGroupQuery, IReadOnlyList<CourseTimetableForGroupResponse>> {
    private readonly ITimetableService _timetableService;
    private readonly IUserContext _userContext;
    
    public GetTimetableForGroupQueryHandler(ITimetableService timetableService, IUserContext userContext) {
        _timetableService = timetableService;
        _userContext = userContext;
    }
    
    public async Task<Result<IReadOnlyList<CourseTimetableForGroupResponse>>> Handle(GetTimetableForGroupQuery request, CancellationToken cancellationToken) {
        var timetableResult = await _timetableService
            .GetTimetableForDate(_userContext.GroupId, request.TimetableDate);

        if (timetableResult.IsFailure) {
            return Result.Failure<IReadOnlyList<CourseTimetableForGroupResponse>>(AttendanceErrors.CouldNotRetrieveTimetable);
        }
        
        var formattedTimetable = timetableResult.Value
            .Select(courseTimetable => new CourseTimetableForGroupResponse {
                Period = courseTimetable.Period,
                CourseName = courseTimetable.CourseName,
                Subgroup = courseTimetable.Subgroup
            }).ToList();

        return formattedTimetable;
    }
}