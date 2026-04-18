using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Abstractions.Timetable;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Courses;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Attendances.GetTimetableForGroup;

internal sealed class GetTimetableForGroupQueryHandler : IQueryHandler<GetTimetableForGroupQuery, IReadOnlyList<CourseTimetableForGroupResponse>> {
    private readonly ITimetableService _timetableService;
    private readonly ICourseRepository _courseRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    
    public GetTimetableForGroupQueryHandler(
        ITimetableService timetableService, 
        IUserContext userContext, 
        ICourseRepository courseRepository, 
        IGroupRepository groupRepository,
        IUserRepository userRepository) 
    {
        _timetableService = timetableService;
        _userContext = userContext;
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Result<IReadOnlyList<CourseTimetableForGroupResponse>>> Handle(GetTimetableForGroupQuery request, CancellationToken cancellationToken) {
        var timetableResult = await _timetableService
            .GetTimetableForDate(_userContext.GroupId, request.TimetableDate, cancellationToken);

        if (timetableResult.IsFailure) {
            return Result.Failure<IReadOnlyList<CourseTimetableForGroupResponse>>(timetableResult.Error);
        }
        
        var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<IReadOnlyList<CourseTimetableForGroupResponse>>(UserErrors.NotFound);
        }
        
        var formattedTimetable = new List<CourseTimetableForGroupResponse>();
        var myGroupCourses = await _courseRepository.GetAllByGroupIdAsync(_userContext.GroupId, cancellationToken);
        var myGroupName = (await _groupRepository.GetByIdAsync(_userContext.GroupId, cancellationToken))?.GroupName;
        var isTheUsersGroupL2 = myGroupName?.Contains("L2") ?? false;

        foreach (var lesson in timetableResult.Value) {
            var course = myGroupCourses.FirstOrDefault(c => {
                var courseName = lesson.CourseName.Substring(6).ToLower();
                return courseName.Contains(c.CourseName.ToLower());
            });
            
            if (course is null) {
                continue;
            }

            if (isTheUsersGroupL2 && lesson.CourseName.Contains("French")) {
                if (!_timetableService.IsLessonIncludedInTimetable(lesson, user))
                    continue;
            }
            
            if (lesson.FacultySubgroup != 0 && lesson.FacultySubgroup != user.FacultySubgroup) {
                continue;
            }
            
            if (lesson.LanguageSubgroup != 0 && lesson.LanguageSubgroup != user.LanguageSubgroup) {
                continue;
            }
            
            formattedTimetable.Add(new CourseTimetableForGroupResponse {
                Period = lesson.Period,
                CourseName = lesson.CourseName,
                CourseId = course.Id,
                FacultySubgroup = lesson.FacultySubgroup,
                LanguageSubgroup = lesson.LanguageSubgroup,
                Teacher = lesson.Teacher
            });
        }

        return formattedTimetable;
    }
}