using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Abstractions.Timetable;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.Courses;
using SkipSmart.Domain.Groups;

namespace SkipSmart.Application.Attendances.GetTimetableForGroup;

internal sealed class GetTimetableForGroupQueryHandler : IQueryHandler<GetTimetableForGroupQuery, IReadOnlyList<CourseTimetableForGroupResponse>> {
    private readonly ITimetableService _timetableService;
    private readonly ICourseRepository _courseRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserContext _userContext;
    
    public GetTimetableForGroupQueryHandler(ITimetableService timetableService, IUserContext userContext, ICourseRepository courseRepository, IGroupRepository groupRepository) {
        _timetableService = timetableService;
        _userContext = userContext;
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
    }
    
    public async Task<Result<IReadOnlyList<CourseTimetableForGroupResponse>>> Handle(GetTimetableForGroupQuery request, CancellationToken cancellationToken) {
        var timetableResult = await _timetableService
            .GetTimetableForDate(_userContext.GroupId, request.TimetableDate, cancellationToken);

        if (timetableResult.IsFailure) {
            return Result.Failure<IReadOnlyList<CourseTimetableForGroupResponse>>(timetableResult.Error);
        }
        
        var formattedTimetable = new List<CourseTimetableForGroupResponse>();
        var myGroupCourses = await _courseRepository.GetAllByGroupIdAsync(_userContext.GroupId, cancellationToken);
        var myGroupName = (await _groupRepository.GetByIdAsync(_userContext.GroupId, cancellationToken))?.GroupName.Value;
        bool isMyGroupL1 = myGroupName?.Contains("L1") ?? false;

        foreach (var lesson in timetableResult.Value) {
            var course = myGroupCourses.FirstOrDefault(c => {
                var courseName = lesson.CourseName.Substring(6).ToLower();
                return courseName.StartsWith(c.CourseName.Value.ToLower());
            });
            
            if (course is null) {
                continue;
            }
            
            formattedTimetable.Add(new CourseTimetableForGroupResponse {
                Period = lesson.Period,
                CourseName = lesson.CourseName,
                CourseId = course.Id,
                FacultySubgroup = lesson.FacultySubgroup,
                LanguageSubgroup = lesson.LanguageSubgroup,
                Teacher = isMyGroupL1 ? lesson.Teacher : string.Empty
            });
        }

        return formattedTimetable;
    }
}