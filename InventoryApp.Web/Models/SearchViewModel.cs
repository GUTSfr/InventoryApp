namespace InventoryApp.Web.Models;

public class SearchViewModel
{
    public string Query { get; set; } = string.Empty;
    public List<InventoryViewModel> Inventories { get; set; } = [];
}   