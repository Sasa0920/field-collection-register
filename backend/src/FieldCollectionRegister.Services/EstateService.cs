using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Services;

public class EstateService : IEstateService
{
    private readonly IEstateRepository _estateRepository;

    public EstateService(IEstateRepository estateRepository)
    {
        _estateRepository = estateRepository;
    }

    public Task<IEnumerable<Estate>> GetEstatesForRegionAsync(int regionId) =>
        _estateRepository.GetByRegionIdAsync(regionId);
}
