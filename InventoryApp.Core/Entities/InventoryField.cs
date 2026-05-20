namespace InventoryApp.Core.Entities;

public enum FieldType
{
    SingleLineText = 1,
    MultiLineText = 2,
    Numeric = 3,
    Link = 4,
    Boolean = 5
}

public class InventoryField
{
    public int Id { get; set; }
    public int InventoryId { get; set; }
    public FieldType Type { get; set; }
    public int SlotIndex { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool ShowInTable { get; set; }
    public int SortOrder { get; set; }

    public Inventory Inventory { get; set; } = null!;
}