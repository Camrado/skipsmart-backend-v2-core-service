using Dapper;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Courses.GetCoursesForGroup;

internal sealed class GetCoursesForGroupQueryHandler : IQueryHandler<GetCoursesForGroupQuery, IReadOnlyList<CourseResponse>> {
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    
    public GetCoursesForGroupQueryHandler(ISqlConnectionFactory sqlConnectionFactory) {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<Result<IReadOnlyList<CourseResponse>>> Handle(GetCoursesForGroupQuery request, CancellationToken cancellationToken) {
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        var sql = """
                  -- noinspection
                  SELECT
                      id as Id,
                      course_name as CourseName,
                      semester as Semester
                  FROM courses
                  WHERE group_id = @GroupId;
                  """;
        
        var courses = await connection.QueryAsync<CourseResponse>(sql, new { request.GroupId });
        
        return courses.ToList();
    }
}