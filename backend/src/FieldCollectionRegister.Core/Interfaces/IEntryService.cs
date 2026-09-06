using FieldCollectionRegister.Core.Dtos;
using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Core.Interfaces;

public interface IEntryService
{
    Task<IEnumerable<CollectionEntry>> GetEntriesAsync(EntryFilter filter);

    // Validation (does the Field exist, is the quantity sane, etc.) lives
    // here in the Services layer, not in the controller and not in the
    // database layer - this is the "business logic behind an interface"
    // layer the onboarding packet describes.
    Task<ServiceResult<CollectionEntry>> CreateEntryAsync(CreateEntryRequest request);
}
