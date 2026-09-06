using FieldCollectionRegister.Core.Dtos;
using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Core.Interfaces;

public interface IEntryRepository
{
    Task<IEnumerable<CollectionEntry>> GetAsync(EntryFilter filter);
    Task<CollectionEntry> CreateAsync(CreateEntryRequest request);
}
