using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Courses.GetCoursesForGroup;

public sealed record GetCoursesForGroupQuery(Guid GroupId) : IQuery<IReadOnlyList<CourseResponse>>;