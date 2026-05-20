namespace InventoryApp.Core.Entities;

public class Item
{
    public int Id { get; set; }
    public int InventoryId { get; set; }
    public string CustomId { get; set; } = string.Empty;
    public int RowVersion { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedById { get; set; } = string.Empty;

    public string? String1 { get; set; }
    public string? String2 { get; set; }
    public string? String3 { get; set; }

    public string? Text1 { get; set; }
    public string? Text2 { get; set; }
    public string? Text3 { get; set; }

    public decimal? Number1 { get; set; }
    public decimal? Number2 { get; set; }
    public decimal? Number3 { get; set; }

    public bool? Bool1 { get; set; }
    public bool? Bool2 { get; set; }
    public bool? Bool3 { get; set; }

    public string? Link1 { get; set; }
    public string? Link2 { get; set; }
    public string? Link3 { get; set; }

    public Inventory Inventory { get; set; } = null!;
    public AppUser CreatedBy { get; set; } = null!;
    public ICollection<Like> Likes { get; set; } = [];
}