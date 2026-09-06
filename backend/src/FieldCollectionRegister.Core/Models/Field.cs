namespace FieldCollectionRegister.Core.Models;

public class Field
{
    public int Id { get; set; }
    public int EstateId { get; set; }
    public string Name { get; set; } = string.Empty;
}