using Dapper;
using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserResponse> {
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IUserContext _userContext;
    
    public GetLoggedInUserQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext) {
        _sqlConnectionFactory = sqlConnectionFactory;
        _userContext = userContext;
    }
    
    public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken) {
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        const string sql = """
                           -- noinspection SqlResolve
                           SELECT
                               u.id AS Id,
                               u.first_name AS FirstName,
                               u.last_name AS LastName,
                               u.email AS Email,
                               u.is_email_verified AS IsEmailVerified,
                               u.language_subgroup AS LanguageSubgroup,
                               u.faculty_subgroup AS FacultySubgroup,
                               g.group_name AS GroupName,
                               u.group_id AS GroupId
                           FROM users u
                           LEFT JOIN groups g ON u.group_id = g.id
                           WHERE u.id = @UserId
                           """;

        var user = await connection.QuerySingleAsync<UserResponse>(sql,
            new {
                _userContext.UserId
            });

        return user;
    }
}