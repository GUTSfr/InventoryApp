using InventoryApp.Core.Entities;

namespace InventoryApp.Web.Models;

public class CustomIdViewModel
{
    public int InventoryId { get; set; }
    public string InventoryTitle { get; set; } = string.Empty;
    public List<CustomIdElementViewModel> Elements { get; set; } = [];
    public string Preview { get; set; } = string.Empty;
}

public class CustomIdElementViewModel
{
    public int Id { get; set; }
    public IdElementType Type { get; set; }
    public string? FixedValue { get; set; }
    public string? FormatPattern { get; set; }
    public int SortOrder { get; set; }
}