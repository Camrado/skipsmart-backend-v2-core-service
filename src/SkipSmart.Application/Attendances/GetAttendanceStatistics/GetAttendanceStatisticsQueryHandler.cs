using Dapper;
using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Attendances.GetAttendanceStatistics;

internal sealed class GetAttendanceStatisticsQueryHandler
    : IQueryHandler<GetAttendanceStatisticsQuery, CourseAttendanceStatisticsResponse> 
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IUserContext _userContext;
    
    public GetAttendanceStatisticsQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext) {
        _sqlConnectionFactory = sqlConnectionFactory;
        _userContext = userContext;
    }
    
    public async Task<Result<CourseAttendanceStatisticsResponse>> Handle(GetAttendanceStatisticsQuery request, CancellationToken cancellationToken) {
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        var sql = """
                  -- noinspection SqlNoDataSourceInspection
                  WITH course_info AS (
                    SELECT
                        ch.course_id,
                        FLOOR(CEIL(ch.hours / 1.5) / 4) AS TotalSkipsAllowedNumber,
                        CEIL(ch.hours / 1.5) AS TotalLessonsNumber
                    FROM 
                        course_hours AS ch
                    WHERE
                        ch.course_id = @CourseId
                  ),
                  attendance_counts AS (
                    SELECT 
                        a.course_id,
                        COUNT(CASE WHEN a.has_attended THEN 1 END) AS AttendedLessonsNumber,
                        COUNT(CASE WHEN NOT a.has_attended THEN 1 END) AS SkippedLessonsNumber
                    FROM
                        attendances as a
                    WHERE
                        a.course_id = @CourseId AND
                        a.user_id = @UserId
                    GROUP BY
                        a.course_id)
                  SELECT
                      @CourseId AS CourseId,
                      COALESCE(ac.AttendedLessonsNumber, 0) AS AttendedLessonsNumber,
                      COALESCE(ac.SkippedLessonsNumber, 0) AS SkippedLessonsNumber,
                      ci.TotalLessonsNumber - COALESCE(ac.AttendedLessonsNumber, 0) - COALESCE(ac.SkippedLessonsNumber, 0) AS RemainingLessonsNumber,
                      ci.TotalSkipsAllowedNumber - COALESCE(ac.SkippedLessonsNumber, 0) AS RemainingSkipsNumber,
                      ci.TotalLessonsNumber AS TotalLessonsNumber
                  FROM
                      course_info AS ci
                  LEFT JOIN attendance_counts AS ac
                      ON ci.course_id = ac.course_id
                  """;

        var courseAttendanceStatus = await connection.QueryFirstOrDefaultAsync<CourseAttendanceStatisticsResponse>(sql, 
            new {
                request.CourseId,
                _userContext.UserId
            });

        return courseAttendanceStatus;
    }
}