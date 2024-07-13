using System.Data;

namespace SkipSmart.Application.Abstractions.Data;

public interface ISqlConnectionFactory {
    IDbConnection CreateConnection();
}