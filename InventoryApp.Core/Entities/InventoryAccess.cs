namespace InventoryApp.Core.Entities;

public class InventoryAccess
{
    public int InventoryId { get; set; }
    public string UserId { get; set; } = string.Empty;

    public Inventory Inventory { get; set; } = null!;
    public AppUser User { get; set; } = null!;
}   