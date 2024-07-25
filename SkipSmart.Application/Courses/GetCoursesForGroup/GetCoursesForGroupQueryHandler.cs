using Dapper;
using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Courses.GetCoursesForGroup;

internal sealed class GetCoursesForGroupQueryHandler : IQueryHandler<GetCoursesForGroupQuery, IReadOnlyList<CourseResponse>> {
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IUserContext _userContext;
    
    public GetCoursesForGroupQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext) {
        _sqlConnectionFactory = sqlConnectionFactory;
        _userContext = userContext;
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
        
        var courses = await connection.QueryAsync<CourseResponse>(sql, new { _userContext.GroupId });
        
        return courses.ToList();
    }
}