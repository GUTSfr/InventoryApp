namespace InventoryApp.Core.Entities;

public enum IdElementType
{
    FixedText = 1,
    Random20Bit = 2,
    Random32Bit = 3,
    Random6Digit = 4,
    Random9Digit = 5,
    Guid = 6,
    DateTime = 7,
    Sequence = 8
}

public class CustomIdElement
{
    public int Id { get; set; }
    public int CustomIdFormatId { get; set; }
    public IdElementType Type { get; set; }
    public string? FixedValue { get; set; }
    public string? FormatPattern { get; set; }
    public int SortOrder { get; set; }

    public CustomIdFormat Format { get; set; } = null!;
}