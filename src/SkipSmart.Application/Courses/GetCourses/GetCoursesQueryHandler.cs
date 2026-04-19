using Dapper;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Courses.GetCourses;

internal sealed class GetCoursesQueryHandler : IQueryHandler<GetCoursesQuery, IReadOnlyList<AdminCourseResponse>> {
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCoursesQueryHandler(ISqlConnectionFactory sqlConnectionFactory) {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<AdminCourseResponse>>> Handle(GetCoursesQuery request, CancellationToken cancellationToken) {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
                  SELECT
                      c.id as Id,
                      c.course_name as CourseName,
                      c.semester as Semester,
                      c.hours as Hours,
                      c.group_id as GroupId,
                      g.group_name as GroupName
                  FROM courses c
                  JOIN groups g ON c.group_id = g.id
                  """;

        if (request.GroupId.HasValue) {
            sql += " WHERE c.group_id = @GroupId";
        }

        var courses = await connection.QueryAsync<AdminCourseResponse>(sql, new { GroupId = request.GroupId });

        return courses.ToList();
    }
}
