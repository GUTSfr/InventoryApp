namespace InventoryApp.Core.Entities;

public class Comment
{
    public int Id { get; set; }
    public int InventoryId { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Inventory Inventory { get; set; } = null!;
    public AppUser Author { get; set; } = null!;
}