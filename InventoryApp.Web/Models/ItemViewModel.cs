using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Web.Models;

public class ItemViewModel
{
    public int Id { get; set; }
    public int InventoryId { get; set; }
    public string CustomId { get; set; } = string.Empty;
    public int RowVersion { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
    public int LikesCount { get; set; }
    public bool UserHasLiked { get; set; }

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

    public List<FieldMetaViewModel> Fields { get; set; } = [];
}

public class FieldMetaViewModel
{
    public string SlotName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool ShowInTable { get; set; }
    public int SortOrder { get; set; }
}