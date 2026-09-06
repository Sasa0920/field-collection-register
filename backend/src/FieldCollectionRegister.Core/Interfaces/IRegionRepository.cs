using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Core.Interfaces;

public interface IRegionRepository
{
    Task<IEnumerable<Region>> GetAllAsync();
    Task<Region?> GetByIdAsync(int id);
}
