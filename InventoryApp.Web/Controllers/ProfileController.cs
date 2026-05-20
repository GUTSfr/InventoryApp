using InventoryApp.Core.Entities;
using InventoryApp.Infrastructure.Data;
using InventoryApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public ProfileController(AppDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);

        var ownedInventories = await _db.Inventories
            .Include(i => i.Category)
            .Include(i => i.Items)
            .Where(i => i.OwnerId == userId)
            .Select(i => new InventoryViewModel
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                IsPublic = i.IsPublic,
                CategoryId = i.CategoryId,
                CategoryName = i.Category.Name,
                CreatedAt = i.CreatedAt,
                ItemCount = i.Items.Count
            })
            .ToListAsync();

        var accessInventories = await _db.InventoryAccesses
            .Include(ia => ia.Inventory)
                .ThenInclude(i => i.Category)
            .Include(ia => ia.Inventory)
                .ThenInclude(i => i.Items)
            .Where(ia => ia.UserId == userId)
            .Select(ia => new InventoryViewModel
            {
                Id = ia.Inventory.Id,
                Title = ia.Inventory.Title,
                Description = ia.Inventory.Description,
                IsPublic = ia.Inventory.IsPublic,
                CategoryId = ia.Inventory.CategoryId,
                CategoryName = ia.Inventory.Category.Name,
                CreatedAt = ia.Inventory.CreatedAt,
                ItemCount = ia.Inventory.Items.Count
            })
            .ToListAsync();

        ViewBag.OwnedInventories = ownedInventories;
        ViewBag.AccessInventories = accessInventories;

        return View();
    }
}