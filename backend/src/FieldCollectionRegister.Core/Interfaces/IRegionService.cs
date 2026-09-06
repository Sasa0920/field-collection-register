using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Core.Interfaces;

public interface IRegionService
{
    Task<IEnumerable<Region>> GetAllRegionsAsync();
}
