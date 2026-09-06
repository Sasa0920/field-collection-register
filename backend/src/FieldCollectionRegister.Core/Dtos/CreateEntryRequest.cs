namespace FieldCollectionRegister.Core.Dtos;

public class CreateEntryRequest
{
    public int FieldId { get; set; }
    public DateOnly CollectionDate { get; set; }
    public decimal Quantity { get; set; }
    public string? Note { get; set; }
}
