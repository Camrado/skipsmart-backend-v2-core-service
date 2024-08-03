using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Abstractions.Timetable;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.Courses;

namespace SkipSmart.Application.Attendances.GetTimetableForGroup;

internal sealed class GetTimetableForGroupQueryHandler : IQueryHandler<GetTimetableForGroupQuery, IReadOnlyList<CourseTimetableForGroupResponse>> {
    private readonly ITimetableService _timetableService;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserContext _userContext;
    
    public GetTimetableForGroupQueryHandler(ITimetableService timetableService, IUserContext userContext, ICourseRepository courseRepository) {
        _timetableService = timetableService;
        _userContext = userContext;
        _courseRepository = courseRepository;
    }
    
    public async Task<Result<IReadOnlyList<CourseTimetableForGroupResponse>>> Handle(GetTimetableForGroupQuery request, CancellationToken cancellationToken) {
        var timetableResult = await _timetableService
            .GetTimetableForDate(_userContext.GroupId, request.TimetableDate, cancellationToken);

        if (timetableResult.IsFailure) {
            return Result.Failure<IReadOnlyList<CourseTimetableForGroupResponse>>(timetableResult.Error);
        }

        var formattedTimetable = new List<CourseTimetableForGroupResponse>();

        foreach (var lesson in timetableResult.Value) {
            var course = await _courseRepository.GetByCourseNameAsync(lesson.CourseName, _userContext.GroupId, cancellationToken);
            
            if (course is null) {
                continue;
            }
    
            formattedTimetable.Add(new CourseTimetableForGroupResponse {
                Period = lesson.Period,
                CourseName = lesson.CourseName,
                CourseId = course.Id,
                FacultySubgroup = lesson.FacultySubgroup,
                LanguageSubgroup = lesson.LanguageSubgroup
            });
        }

        return formattedTimetable;
    }
}