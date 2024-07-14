using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Statistics.GetAttendanceStatusForCourse;

public sealed record GetAttendanceStatusForCourseQuery(Guid CourseId) : IQuery<CourseAttendanceStatusResponse>;