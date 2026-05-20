namespace InventoryApp.Core.Entities;

public class InventoryTag
{
    public int InventoryId { get; set; }
    public int TagId { get; set; }

    public Inventory Inventory { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}