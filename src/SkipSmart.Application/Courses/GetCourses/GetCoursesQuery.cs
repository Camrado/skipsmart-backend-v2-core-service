using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Courses.GetCourses;

public sealed record GetCoursesQuery(Guid? GroupId) : IQuery<IReadOnlyList<AdminCourseResponse>>;

public sealed record AdminCourseResponse(Guid Id, string CourseName, int Semester, decimal Hours, Guid GroupId, string GroupName);
