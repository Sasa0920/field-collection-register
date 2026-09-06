using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Core.Interfaces;

public interface IEstateService
{
    // Returns an empty list (not an error) when the region has no estates
    // or the regionId doesn't exist - keeps the dropdown-chain UI simple.
    Task<IEnumerable<Estate>> GetEstatesForRegionAsync(int regionId);
}
