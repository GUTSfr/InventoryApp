using InventoryApp.Infrastructure.Data;
using InventoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Web.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var latestInventories = await _db.Inventories
            .Include(i => i.Owner)
            .Include(i => i.Category)
            .Include(i => i.Items)
            .OrderByDescending(i => i.CreatedAt)
            .Take(10)
            .Select(i => new InventoryViewModel
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                CategoryName = i.Category.Name,
                OwnerName = i.Owner.DisplayName,
                CreatedAt = i.CreatedAt,
                ItemCount = i.Items.Count
            })
            .ToListAsync();

        var popularInventories = await _db.Inventories
            .Include(i => i.Owner)
            .Include(i => i.Category)
            .Include(i => i.Items)
            .OrderByDescending(i => i.Items.Count)
            .Take(5)
            .Select(i => new InventoryViewModel
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                CategoryName = i.Category.Name,
                OwnerName = i.Owner.DisplayName,
                CreatedAt = i.CreatedAt,
                ItemCount = i.Items.Count
            })
            .ToListAsync();

        var tags = await _db.Tags
            .Include(t => t.InventoryTags)
            .Select(t => new { t.Name, Count = t.InventoryTags.Count })
            .OrderByDescending(t => t.Count)
            .Take(20)
            .ToListAsync();

        ViewBag.LatestInventories = latestInventories;
        ViewBag.PopularInventories = popularInventories;
        ViewBag.Tags = tags;

        return View();
    }
}