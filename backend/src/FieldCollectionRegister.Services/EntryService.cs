using FieldCollectionRegister.Core.Dtos;
using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.Core.Models;

namespace FieldCollectionRegister.Services;

public class EntryService : IEntryService
{
    private readonly IEntryRepository _entryRepository;
    private readonly IFieldRepository _fieldRepository;

    public EntryService(IEntryRepository entryRepository, IFieldRepository fieldRepository)
    {
        _entryRepository = entryRepository;
        _fieldRepository = fieldRepository;
    }

    public Task<IEnumerable<CollectionEntry>> GetEntriesAsync(EntryFilter filter) =>
        _entryRepository.GetAsync(filter);

    public async Task<ServiceResult<CollectionEntry>> CreateEntryAsync(CreateEntryRequest request)
    {
        // Never trust input from the client - re-validate here even though
        // the frontend form already checks this. This is the same "don't
        // rely on the frontend hiding things" principle the onboarding
        // packet asks about for permissions.
        if (request.FieldId <= 0)
        {
            return ServiceResult<CollectionEntry>.Fail("A field must be selected.");
        }

        var field = await _fieldRepository.GetByIdAsync(request.FieldId);
        if (field is null)
        {
            return ServiceResult<CollectionEntry>.Fail("The selected field does not exist.");
        }

        if (request.Quantity <= 0)
        {
            return ServiceResult<CollectionEntry>.Fail("Quantity must be greater than zero.");
        }

        if (request.CollectionDate == default)
        {
            return ServiceResult<CollectionEntry>.Fail("A collection date is required.");
        }

        if (request.CollectionDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return ServiceResult<CollectionEntry>.Fail("Collection date cannot be in the future.");
        }

        var created = await _entryRepository.CreateAsync(request);
        return ServiceResult<CollectionEntry>.Ok(created);
    }
}
