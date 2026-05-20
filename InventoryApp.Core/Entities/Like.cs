namespace InventoryApp.Core.Entities;

public class Like
{
    public int ItemId { get; set; }
    public string UserId { get; set; } = string.Empty;

    public Item Item { get; set; } = null!;
    public AppUser User { get; set; } = null!;
}