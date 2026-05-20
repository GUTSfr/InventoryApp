using InventoryApp.Core.Entities;
using InventoryApp.Infrastructure.Data;
using InventoryApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Web.Controllers;

public class ItemController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public ItemController(AppDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.Items
            .Include(i => i.CreatedBy)
            .Include(i => i.Inventory)
                .ThenInclude(inv => inv.Fields)
            .Include(i => i.Likes)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);

        var model = new ItemViewModel
        {
            Id = item.Id,
            InventoryId = item.InventoryId,
            CustomId = item.CustomId,
            CreatedAt = item.CreatedAt,
            CreatedByName = item.CreatedBy.DisplayName,
            LikesCount = item.Likes.Count,
            UserHasLiked = item.Likes.Any(l => l.UserId == userId),
            RowVersion = item.RowVersion,
            String1 = item.String1,
            String2 = item.String2,
            String3 = item.String3,
            Text1 = item.Text1,
            Text2 = item.Text2,
            Text3 = item.Text3,
            Number1 = item.Number1,
            Number2 = item.Number2,
            Number3 = item.Number3,
            Bool1 = item.Bool1,
            Bool2 = item.Bool2,
            Bool3 = item.Bool3,
            Link1 = item.Link1,
            Link2 = item.Link2,
            Link3 = item.Link3,
            Fields = item.Inventory.Fields
                .OrderBy(f => f.SortOrder)
                .Select(f => new FieldMetaViewModel
                {
                    SlotName = $"{f.Type}{f.SlotIndex}",
                    Title = f.Title,
                    Description = f.Description,
                    Type = f.Type.ToString(),
                    ShowInTable = f.ShowInTable,
                    SortOrder = f.SortOrder
                })
                .ToList()
        };

        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> Create(int inventoryId)
    {
        var inventory = await _db.Inventories
            .Include(i => i.Fields)
            .FirstOrDefaultAsync(i => i.Id == inventoryId);

        if (inventory == null)
            return NotFound();

        var model = new ItemViewModel
        {
            InventoryId = inventoryId,
            Fields = inventory.Fields
                .OrderBy(f => f.SortOrder)
                .Select(f => new FieldMetaViewModel
                {
                    SlotName = $"{f.Type}{f.SlotIndex}",
                    Title = f.Title,
                    Description = f.Description,
                    Type = f.Type.ToString(),
                    ShowInTable = f.ShowInTable,
                    SortOrder = f.SortOrder
                })
                .ToList()
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(ItemViewModel model)
    {
        var inventory = await _db.Inventories
            .Include(i => i.Fields)
            .Include(i => i.CustomIdFormat)
                .ThenInclude(f => f!.Elements)
            .FirstOrDefaultAsync(i => i.Id == model.InventoryId);

        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        var customId = GenerateCustomId(inventory);

        var item = new Item
        {
            InventoryId = model.InventoryId,
            CustomId = customId,
            CreatedById = userId!,
            RowVersion = 1,
            String1 = model.String1,
            String2 = model.String2,
            String3 = model.String3,
            Text1 = model.Text1,
            Text2 = model.Text2,
            Text3 = model.Text3,
            Number1 = model.Number1,
            Number2 = model.Number2,
            Number3 = model.Number3,
            Bool1 = model.Bool1,
            Bool2 = model.Bool2,
            Bool3 = model.Bool3,
            Link1 = model.Link1,
            Link2 = model.Link2,
            Link3 = model.Link3
        };

        _db.Items.Add(item);

        try
        {
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", "Inventory", new { id = model.InventoryId });
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty, "ID conflict. Please try again.");
            model.Fields = inventory.Fields
                .OrderBy(f => f.SortOrder)
                .Select(f => new FieldMetaViewModel
                {
                    SlotName = $"{f.Type}{f.SlotIndex}",
                    Title = f.Title,
                    Description = f.Description,
                    Type = f.Type.ToString(),
                    ShowInTable = f.ShowInTable,
                    SortOrder = f.SortOrder
                })
                .ToList();
            return View(model);
        }
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.Items
            .Include(i => i.Inventory)
                .ThenInclude(inv => inv.Fields)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
            return NotFound();

        var model = new ItemViewModel
        {
            Id = item.Id,
            InventoryId = item.InventoryId,
            CustomId = item.CustomId,
            RowVersion = item.RowVersion,
            String1 = item.String1,
            String2 = item.String2,
            String3 = item.String3,
            Text1 = item.Text1,
            Text2 = item.Text2,
            Text3 = item.Text3,
            Number1 = item.Number1,
            Number2 = item.Number2,
            Number3 = item.Number3,
            Bool1 = item.Bool1,
            Bool2 = item.Bool2,
            Bool3 = item.Bool3,
            Link1 = item.Link1,
            Link2 = item.Link2,
            Link3 = item.Link3,
            Fields = item.Inventory.Fields
                .OrderBy(f => f.SortOrder)
                .Select(f => new FieldMetaViewModel
                {
                    SlotName = $"{f.Type}{f.SlotIndex}",
                    Title = f.Title,
                    Description = f.Description,
                    Type = f.Type.ToString(),
                    ShowInTable = f.ShowInTable,
                    SortOrder = f.SortOrder
                })
                .ToList()
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(ItemViewModel model)
    {
        var item = await _db.Items
            .Include(i => i.Inventory)
                .ThenInclude(inv => inv.Fields)
            .FirstOrDefaultAsync(i => i.Id == model.Id);

        if (item == null)
            return NotFound();

        if (item.RowVersion != model.RowVersion)
        {
            ModelState.AddModelError(string.Empty, "This item was modified by someone else. Please reload and try again.");
            model.Fields = item.Inventory.Fields
                .OrderBy(f => f.SortOrder)
                .Select(f => new FieldMetaViewModel
                {
                    SlotName = $"{f.Type}{f.SlotIndex}",
                    Title = f.Title,
                    Description = f.Description,
                    Type = f.Type.ToString(),
                    ShowInTable = f.ShowInTable,
                    SortOrder = f.SortOrder
                })
                .ToList();
            return View(model);
        }

        item.CustomId = model.CustomId;
        item.String1 = model.String1;
        item.String2 = model.String2;
        item.String3 = model.String3;
        item.Text1 = model.Text1;
        item.Text2 = model.Text2;
        item.Text3 = model.Text3;
        item.Number1 = model.Number1;
        item.Number2 = model.Number2;
        item.Number3 = model.Number3;
        item.Bool1 = model.Bool1;
        item.Bool2 = model.Bool2;
        item.Bool3 = model.Bool3;
        item.Link1 = model.Link1;
        item.Link2 = model.Link2;
        item.Link3 = model.Link3;
        item.RowVersion += 1;
        item.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = item.Id });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(int id, int inventoryId)
    {
        var item = await _db.Items.FindAsync(id);
        if (item != null)
        {
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Details", "Inventory", new { id = inventoryId });
    }

    private string GenerateCustomId(Inventory inventory)
    {
        if (inventory.CustomIdFormat == null || !inventory.CustomIdFormat.Elements.Any())
            return Guid.NewGuid().ToString("N")[..8].ToUpper();

        var parts = inventory.CustomIdFormat.Elements
            .OrderBy(e => e.SortOrder)
            .Select(e => GenerateElement(e, inventory.Id));

        return string.Concat(parts);
    }

    private string GenerateElement(CustomIdElement element, int inventoryId)
    {
        return element.Type switch
        {
            IdElementType.FixedText => element.FixedValue ?? string.Empty,
            IdElementType.Random20Bit => Convert.ToString(Random.Shared.Next(0, 1048576), 16).ToUpper().PadLeft(5, '0'),
            IdElementType.Random32Bit => Convert.ToString(Random.Shared.Next(0, int.MaxValue), 16).ToUpper().PadLeft(8, '0'),
            IdElementType.Random6Digit => Random.Shared.Next(0, 999999).ToString("D6"),
            IdElementType.Random9Digit => Random.Shared.Next(0, 999999999).ToString("D9"),
            IdElementType.Guid => Guid.NewGuid().ToString("N").ToUpper(),
            IdElementType.DateTime => DateTime.UtcNow.ToString(element.FormatPattern ?? "yyyy"),
            IdElementType.Sequence => GetNextSequence(inventoryId).ToString(element.FormatPattern ?? "D"),
            _ => string.Empty
        };
    }

    private int GetNextSequence(int inventoryId)
    {
        return _db.Items.Where(i => i.InventoryId == inventoryId).Count() + 1;
    }
}