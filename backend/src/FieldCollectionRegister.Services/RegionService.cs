using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Services;

public class RegionService : IRegionService
{
    private readonly IRegionRepository _regionRepository;

    public RegionService(IRegionRepository regionRepository)
    {
        _regionRepository = regionRepository;
    }

    public Task<IEnumerable<Region>> GetAllRegionsAsync() => _regionRepository.GetAllAsync();
}
