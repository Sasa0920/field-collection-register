using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Core.Interfaces;

public interface IEstateRepository
{
    Task<IEnumerable<Estate>> GetByRegionIdAsync(int regionId);
    Task<Estate?> GetByIdAsync(int id);
}
