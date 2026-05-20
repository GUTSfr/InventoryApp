namespace InventoryApp.Web.Models;

public class AccessViewModel
{
    public int InventoryId { get; set; }
    public string InventoryTitle { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public List<UserViewModel> UsersWithAccess { get; set; } = [];
    public string? NewUserEmail { get; set; }
}