using System.Text;
using Dapper;
using FieldCollectionRegister.Core.Dtos;
using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.DataAccess;

public class EntryRepository : IEntryRepository
{
    private readonly SqliteConnectionFactory _factory;

    public EntryRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<CollectionEntry>> GetAsync(EntryFilter filter)
    {
        using var connection = _factory.CreateConnection();

        // Base query joins up through the hierarchy so the frontend gets
        // readable names, not just raw ids, in a single round trip.
        var sql = new StringBuilder("""
            SELECT
                e.Id, e.FieldId, e.CollectionDate, e.Quantity, e.Note,
                f.Name AS FieldName,
                es.Id AS EstateId, es.Name AS EstateName,
                r.Id AS RegionId, r.Name AS RegionName
            FROM Entries e
            JOIN Fields f ON f.Id = e.FieldId
            JOIN Estates es ON es.Id = f.EstateId
            JOIN Regions r ON r.Id = es.RegionId
            WHERE 1 = 1
            """);

        var parameters = new DynamicParameters();

        // Every value that ends up in the query is bound as a parameter,
        // never string-concatenated - same discipline as the real stack's
        // parameterized stored procedure calls.
        if (filter.RegionId is not null)
        {
            sql.Append(" AND r.Id = @RegionId");
            parameters.Add("RegionId", filter.RegionId);
        }

        if (filter.EstateId is not null)
        {
            sql.Append(" AND es.Id = @EstateId");
            parameters.Add("EstateId", filter.EstateId);
        }

        if (filter.FieldId is not null)
        {
            sql.Append(" AND f.Id = @FieldId");
            parameters.Add("FieldId", filter.FieldId);
        }

        sql.Append(" ORDER BY e.CollectionDate DESC, e.Id DESC;");

        return await connection.QueryAsync<CollectionEntry>(sql.ToString(), parameters);
    }

    public async Task<CollectionEntry> CreateAsync(CreateEntryRequest request)
    {
        using var connection = _factory.CreateConnection();

        const string insertSql = """
            INSERT INTO Entries (FieldId, CollectionDate, Quantity, Note)
            VALUES (@FieldId, @CollectionDate, @Quantity, @Note);
            SELECT last_insert_rowid();
            """;

        var newId = await connection.ExecuteScalarAsync<long>(insertSql, new
        {
            request.FieldId,
            CollectionDate = request.CollectionDate.ToString("yyyy-MM-dd"),
            request.Quantity,
            request.Note
        });

        const string readBackSql = """
            SELECT
                e.Id, e.FieldId, e.CollectionDate, e.Quantity, e.Note,
                f.Name AS FieldName,
                es.Id AS EstateId, es.Name AS EstateName,
                r.Id AS RegionId, r.Name AS RegionName
            FROM Entries e
            JOIN Fields f ON f.Id = e.FieldId
            JOIN Estates es ON es.Id = f.EstateId
            JOIN Regions r ON r.Id = es.RegionId
            WHERE e.Id = @Id;
            """;

        return await connection.QuerySingleAsync<CollectionEntry>(readBackSql, new { Id = newId });
    }
}
