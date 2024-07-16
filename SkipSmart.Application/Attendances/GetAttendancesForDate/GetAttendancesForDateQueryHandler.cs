using Dapper;
using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Attendances.GetAttendancesForDate;

internal sealed class GetAttendancesForDateQueryHandler : IQueryHandler<GetAttendancesForDateQuery, IReadOnlyList<AttendanceForDateResponse>> {
    private readonly IUserContext _userContext;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    
    public GetAttendancesForDateQueryHandler(IUserContext userContext, ISqlConnectionFactory sqlConnectionFactory) {
        _userContext = userContext;
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<Result<IReadOnlyList<AttendanceForDateResponse>>> Handle(GetAttendancesForDateQuery request, CancellationToken cancellationToken) {
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        var sql = """
                  --
                  SELECT 
                    period AS Period,
                    has_attended AS HasAttended
                  FROM attendances
                  WHERE
                    user_id = @UserId AND 
                    attendance_date = @AttendanceDate
                  """;
        
        var attendances = await connection.QueryAsync<AttendanceForDateResponse>(sql, new {
            _userContext.UserId,
            request.AttendanceDate
        });

        return attendances.ToList();
    }
}