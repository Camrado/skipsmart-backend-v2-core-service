using System.Data;
using Npgsql;
using SkipSmart.Application.Abstractions.Data;

namespace SkipSmart.Infrastructure.Data;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory {
    private readonly string _connectionString;
    
    public SqlConnectionFactory(string connectionString) {
        _connectionString = connectionString;
    }
    
    public IDbConnection CreateConnection() {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        
        return connection;
    }
}