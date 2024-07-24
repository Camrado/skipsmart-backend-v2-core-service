using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Attendances.GetAttendanceStatistics;

public sealed record GetAttendanceStatisticsQuery(Guid CourseId) : IQuery<CourseAttendanceStatisticsResponse>;