using Dapper;
using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Clock;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Abstractions.Timetable;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Attendances;

namespace SkipSmart.Application.Attendances.GetUnmarkedDates;

internal sealed class GetUnmarkedDatesQueryHandler : IQueryHandler<GetUnmarkedDatesQuery, IReadOnlyList<DateOnly>> {
    private readonly IUserContext _userContext;
    private readonly ITimetableService _timetableService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    
    public GetUnmarkedDatesQueryHandler(
        IUserContext userContext, 
        ITimetableService timetableService, 
        IDateTimeProvider dateTimeProvider, 
        ISqlConnectionFactory sqlConnectionFactory) 
    {
        _userContext = userContext;
        _timetableService = timetableService;
        _dateTimeProvider = dateTimeProvider;
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<Result<IReadOnlyList<DateOnly>>> Handle(GetUnmarkedDatesQuery request, CancellationToken cancellationToken) {
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        var sql = """
                  -- 
                  SELECT
                    recorded_date AS MarkedDate
                  FROM 
                    marked_dates
                  WHERE
                    user_id = @UserId
                  """;
        
        var markedDatesResponse = await connection.QueryAsync<MarkedDateResponse>(sql, new {
            _userContext.UserId
        });
        var markedDates = markedDatesResponse.Select(data => data.MarkedDate);
        
        var startDate = _dateTimeProvider.TodayInBaku.AddMonths(-1).AddDays(-15) > _dateTimeProvider.SemesterStartDate
            ? _dateTimeProvider.TodayInBaku.AddMonths(-1).AddDays(-15)
            : _dateTimeProvider.SemesterStartDate;
        
        if (startDate >= _dateTimeProvider.TodayInBaku.AddDays(-1)) {
            return new List<DateOnly>();
        }
        
        var workingDaysResult = await _timetableService
            .GetWorkingDaysForRange(_userContext.UserId, startDate, _dateTimeProvider.TodayInBaku.AddDays(-1), cancellationToken);
        
        if (workingDaysResult.IsFailure) {
            return Result.Failure<IReadOnlyList<DateOnly>>(workingDaysResult.Error);
        }

        var unmarkedDates = workingDaysResult.Value.Except(markedDates);

        return unmarkedDates.ToList();
    }
}