using System.Xml.Linq;

namespace InventoryApp.Core.Entities;

public class Inventory
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublic { get; set; }
    public int CategoryId { get; set; }
    public int RowVersion { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string OwnerId { get; set; } = string.Empty;
    public AppUser Owner { get; set; } = null!;
    public Category Category { get; set; } = null!;

    public ICollection<InventoryField> Fields { get; set; } = [];
    public ICollection<InventoryTag> InventoryTags { get; set; } = [];
    public ICollection<InventoryAccess> AccessList { get; set; } = [];
    public ICollection<Item> Items { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public CustomIdFormat? CustomIdFormat { get; set; }
}   