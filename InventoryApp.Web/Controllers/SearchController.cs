using InventoryApp.Infrastructure.Data;
using InventoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Web.Controllers;

public class SearchController : Controller
{
    private readonly AppDbContext _db;

    public SearchController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string query)
    {
        var model = new SearchViewModel { Query = query };

        if (string.IsNullOrWhiteSpace(query))
            return View(model);

        model.Inventories = await _db.Inventories
            .Include(i => i.Owner)
            .Include(i => i.Category)
            .Include(i => i.Items)
            .Include(i => i.InventoryTags)
                .ThenInclude(it => it.Tag)
            .Where(i => i.Title.Contains(query) ||
                        (i.Description != null && i.Description.Contains(query)) ||
                        i.InventoryTags.Any(it => it.Tag.Name.Contains(query)))
            .Select(i => new InventoryViewModel
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                IsPublic = i.IsPublic,
                CategoryId = i.CategoryId,
                CategoryName = i.Category.Name,
                ImageUrl = i.ImageUrl,
                OwnerName = i.Owner.DisplayName,
                CreatedAt = i.CreatedAt,
                ItemCount = i.Items.Count,
                Tags = i.InventoryTags.Select(it => it.Tag.Name).ToList()
            })
            .ToListAsync();

        return View(model);
    }
}