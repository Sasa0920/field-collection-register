using Dapper;
using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.DataAccess;

public class RegionRepository : IRegionRepository
{
    private readonly SqliteConnectionFactory _factory;

    public RegionRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<Region>> GetAllAsync()
    {
        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<Region>(
            "SELECT Id, Name FROM Regions ORDER BY Name;");
    }

    public async Task<Region?> GetByIdAsync(int id)
    {
        using var connection = _factory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Region>(
            "SELECT Id, Name FROM Regions WHERE Id = @Id;",
            new { Id = id });
    }
}
