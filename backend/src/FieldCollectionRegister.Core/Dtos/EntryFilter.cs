namespace FieldCollectionRegister.Core.Dtos;

// Optional filters for listing entries. Null means "don't filter on this level".
public class EntryFilter
{
    public int? RegionId { get; set; }
    public int? EstateId { get; set; }
    public int? FieldId { get; set; }
}
