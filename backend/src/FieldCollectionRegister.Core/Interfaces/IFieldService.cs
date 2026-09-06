using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Core.Interfaces;

public interface IFieldService
{
    Task<IEnumerable<Field>> GetFieldsForEstateAsync(int estateId);
}
