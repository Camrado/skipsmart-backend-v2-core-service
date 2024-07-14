using Dapper;
using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Statistics.GetAttendanceStatusForCourse;

internal sealed class GetAttendanceStatusForCourseQueryHandler
    : IQueryHandler<GetAttendanceStatusForCourseQuery, CourseAttendanceStatusResponse> 
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IUserContext _userContext;
    
    public GetAttendanceStatusForCourseQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext) {
        _sqlConnectionFactory = sqlConnectionFactory;
        _userContext = userContext;
    }
    
    public async Task<Result<CourseAttendanceStatusResponse>> Handle(GetAttendanceStatusForCourseQuery request, CancellationToken cancellationToken) {
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        var sql = """
                  -- noinspection SqlNoDataSourceInspection
                  SELECT
                      @CourseId AS CourseId,
                      COUNT(CASE WHEN a.has_attended THEN 1 END) AS AttendedLessonsNumber,
                      COUNT(CASE WHEN NOT a.has_attended THEN 1 END) AS SkippedLessonsNumber,
                      CEIL(c.hours / 1.5) - COUNT(*) AS RemainingLessonsNumber,
                      FLOOR(CEIL(c.hours / 1.5) / 4) - COUNT(CASE WHEN NOT a.has_attended THEN 1 END) AS RemainingSkipsNumber,
                      CEIL(c.hours / 1.5) AS TotalLessonsNumber
                  FROM
                      attendances AS a
                  LEFT JOIN (SELECT ch.course_id, ch.hours AS hours FROM course_hours ch WHERE ch.course_id = @CourseId) AS c 
                      ON c.course_id = a.course_id
                  WHERE
                      a.course_id = @CourseId AND
                      a.user_id = @UserId
                  GROUP BY
                      c.hours
                  """;

        var courseAttendanceStatus = await connection.QueryFirstOrDefaultAsync<CourseAttendanceStatusResponse>(sql, 
            new {
                request.CourseId,
                _userContext.UserId
            });

        return courseAttendanceStatus;
    }
}