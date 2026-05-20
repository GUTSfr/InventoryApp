using InventoryApp.Core.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Web.Controllers;

public class CommentController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public CommentController(AppDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add(int inventoryId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return RedirectToAction("Details", "Inventory", new { id = inventoryId });

        var userId = _userManager.GetUserId(User);

        var comment = new Comment
        {
            InventoryId = inventoryId,
            AuthorId = userId!,
            Content = content
        };

        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();

        return RedirectToAction("Details", "Inventory", new { id = inventoryId });
    }
}