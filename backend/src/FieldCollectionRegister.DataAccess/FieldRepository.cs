using Dapper;
using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.DataAccess;

public class FieldRepository : IFieldRepository
{
    private readonly SqliteConnectionFactory _factory;

    public FieldRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<Field>> GetByEstateIdAsync(int estateId)
    {
        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<Field>(
            "SELECT Id, EstateId, Name FROM Fields WHERE EstateId = @EstateId ORDER BY Name;",
            new { EstateId = estateId });
    }

    public async Task<Field?> GetByIdAsync(int id)
    {
        using var connection = _factory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Field>(
            "SELECT Id, EstateId, Name FROM Fields WHERE Id = @Id;",
            new { Id = id });
    }
}
