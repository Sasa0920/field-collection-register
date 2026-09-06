using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Core.Interfaces;

public interface IFieldRepository
{
    Task<IEnumerable<Field>> GetByEstateIdAsync(int estateId);
    Task<Field?> GetByIdAsync(int id);
}
