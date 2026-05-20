using Microsoft.AspNetCore.Identity;

namespace InventoryApp.Core.Entities;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Inventory> OwnedInventories { get; set; } = [];
    public ICollection<InventoryAccess> GrantedAccesses { get; set; } = [];
    public ICollection<Item> CreatedItems { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<Like> Likes { get; set; } = [];
}