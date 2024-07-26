using Dapper;
using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Clock;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Courses.GetCoursesForGroup;

internal sealed class GetCoursesForGroupQueryHandler : IQueryHandler<GetCoursesForGroupQuery, IReadOnlyList<CourseResponse>> {
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserContext _userContext;
    
    public GetCoursesForGroupQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IDateTimeProvider dateTimeProvider, IUserContext userContext) {
        _sqlConnectionFactory = sqlConnectionFactory;
        _dateTimeProvider = dateTimeProvider;
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
                  WHERE group_id = @GroupId AND semester = @Semester;
                  """;
        
        int semester = _dateTimeProvider.TodayInBaku >= _dateTimeProvider.FirstSemesterStartDate.AddMonths(-1) &&
                       _dateTimeProvider.TodayInBaku < _dateTimeProvider.SecondSemesterStartDate.AddDays(-5) ? 1 : 2;
        
        var courses = await connection.QueryAsync<CourseResponse>(sql, new { _userContext.GroupId, semester });
        
        return courses.ToList();
    }
}