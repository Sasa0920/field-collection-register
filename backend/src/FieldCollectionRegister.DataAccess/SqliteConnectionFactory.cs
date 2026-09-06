using Microsoft.Data.Sqlite;

namespace FieldCollectionRegister.DataAccess;

// A tiny factory so every repository asks for a connection the same way,
// instead of each one holding its own copy of the connection string.
public class SqliteConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqliteConnection CreateConnection() => new(_connectionString);
}
