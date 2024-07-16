using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Attendances.GetAttendanceStatusForCourse;

public sealed record GetAttendanceStatusForCourseQuery(Guid CourseId) : IQuery<CourseAttendanceStatusResponse>;