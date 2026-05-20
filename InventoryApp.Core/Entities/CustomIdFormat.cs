namespace InventoryApp.Core.Entities;

public class CustomIdFormat
{
    public int Id { get; set; }
    public int InventoryId { get; set; }

    public Inventory Inventory { get; set; } = null!;
    public ICollection<CustomIdElement> Elements { get; set; } = [];
}