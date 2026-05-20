using InventoryApp.Core.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Web.Controllers;

public class LikeController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public LikeController(AppDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Toggle(int itemId, int inventoryId)
    {
        var userId = _userManager.GetUserId(User);

        var existingLike = await _db.Likes
            .FirstOrDefaultAsync(l => l.ItemId == itemId && l.UserId == userId);

        if (existingLike != null)
            _db.Likes.Remove(existingLike);
        else
            _db.Likes.Add(new Like { ItemId = itemId, UserId = userId! });

        await _db.SaveChangesAsync();

        return RedirectToAction("Details", "Inventory", new { id = inventoryId });
    }
}