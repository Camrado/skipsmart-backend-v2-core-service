using Dapper;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Groups.GetAllGroups;

internal sealed class GetAllGroupsQueryHandler : IQueryHandler<GetAllGroupsQuery, IReadOnlyList<GroupResponse>> {
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    
    public GetAllGroupsQueryHandler(ISqlConnectionFactory sqlConnectionFactory) {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<Result<IReadOnlyList<GroupResponse>>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken) {
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        var sql = """
                  -- noinspection
                  SELECT
                      id as Id,
                      group_name as GroupName,
                      edupage_class_id as EdupageClassId
                  FROM groups;
                  """;
        
        var groups = await connection.QueryAsync<GroupResponse>(sql);

        return groups.ToList();
    }
}