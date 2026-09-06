using System.Data;
using Dapper;

namespace FieldCollectionRegister.DataAccess;

// SQLite has no native DATE type - it stores dates as TEXT. Dapper can't
// automatically convert TEXT <-> DateOnly, so this handler teaches it how.
public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToString("yyyy-MM-dd");
    }

    public override DateOnly Parse(object value)
    {
        return DateOnly.Parse((string)value);
    }
}