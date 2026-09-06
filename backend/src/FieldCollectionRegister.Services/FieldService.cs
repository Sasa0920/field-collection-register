using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Services;

public class FieldService : IFieldService
{
    private readonly IFieldRepository _fieldRepository;

    public FieldService(IFieldRepository fieldRepository)
    {
        _fieldRepository = fieldRepository;
    }

    public Task<IEnumerable<Field>> GetFieldsForEstateAsync(int estateId) =>
        _fieldRepository.GetByEstateIdAsync(estateId);
}
