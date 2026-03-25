using System.Data;
using Dapper;

namespace SkipSmart.Infrastructure.Data;

internal sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly> {
    public override void SetValue(IDbDataParameter parameter, DateOnly value) {
        parameter.DbType = DbType.Date;
        parameter.Value = value;
    }

    public override DateOnly Parse(object value) => value switch {
        DateTime dt => DateOnly.FromDateTime(dt),
        DateOnly d => d,
        _ => throw new InvalidCastException($"Cannot cast {value.GetType()} to DateOnly.")
    };
}