using Dapper;
using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.DataAccess;

public class EstateRepository : IEstateRepository
{
    private readonly SqliteConnectionFactory _factory;

    public EstateRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<Estate>> GetByRegionIdAsync(int regionId)
    {
        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<Estate>(
            "SELECT Id, RegionId, Name FROM Estates WHERE RegionId = @RegionId ORDER BY Name;",
            new { RegionId = regionId });
    }

    public async Task<Estate?> GetByIdAsync(int id)
    {
        using var connection = _factory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Estate>(
            "SELECT Id, RegionId, Name FROM Estates WHERE Id = @Id;",
            new { Id = id });
    }
}
