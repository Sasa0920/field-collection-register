namespace FieldCollectionRegister.Core.Models;

public class CollectionEntry
{
    public int Id { get; set; }
    public int FieldId { get; set; }
    public DateOnly CollectionDate { get; set; }
    public decimal Quantity { get; set; }
    public string? Note { get; set; }

    // Extra display fields filled in by a join, so the frontend
    // doesn't need separate calls just to show readable names.
    public string? FieldName { get; set; }
    public string? EstateName { get; set; }
    public string? RegionName { get; set; }
}