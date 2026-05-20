using InventoryApp.Core.Entities;
using InventoryApp.Infrastructure.Data;
using InventoryApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Web.Controllers;

public class InventoryController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public InventoryController(AppDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var inventories = await _db.Inventories
            .Include(i => i.Owner)
            .Include(i => i.Category)
            .Include(i => i.Items)
            .Include(i => i.InventoryTags)
                .ThenInclude(it => it.Tag)
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

        return View(inventories);
    }

    [Authorize]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(InventoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
            return View(model);
        }

        var userId = _userManager.GetUserId(User);

        var inventory = new Inventory
        {
            Title = model.Title,
            Description = model.Description,
            IsPublic = model.IsPublic,
            CategoryId = model.CategoryId,
            ImageUrl = model.ImageUrl,
            OwnerId = userId!,
            RowVersion = 1
        };

        _db.Inventories.Add(inventory);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = inventory.Id });
    }

    public async Task<IActionResult> Details(int id)
    {
        var inventory = await _db.Inventories
            .Include(i => i.Owner)
            .Include(i => i.Category)
            .Include(i => i.Items)
                .ThenInclude(i => i.CreatedBy)
            .Include(i => i.InventoryTags)
                .ThenInclude(it => it.Tag)
            .Include(i => i.Comments)
                .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inventory == null)
            return NotFound();

        var model = new InventoryViewModel
        {
            Id = inventory.Id,
            Title = inventory.Title,
            Description = inventory.Description,
            IsPublic = inventory.IsPublic,
            CategoryId = inventory.CategoryId,
            CategoryName = inventory.Category.Name,
            ImageUrl = inventory.ImageUrl,
            OwnerName = inventory.Owner.DisplayName,
            CreatedAt = inventory.CreatedAt,
            ItemCount = inventory.Items.Count,
            RowVersion = inventory.RowVersion,
            Tags = inventory.InventoryTags.Select(it => it.Tag.Name).ToList(),

            TagsWithId = inventory.InventoryTags.Select(it => new TagViewModel
            {
                Id = it.TagId,
                Name = it.Tag.Name
            }).ToList(),
            Items = inventory.Items.Select(i => new ItemViewModel
            {
                Id = i.Id,
                CustomId = i.CustomId,
                CreatedAt = i.CreatedAt,
                CreatedByName = i.CreatedBy.DisplayName
            }).ToList(),
            Comments = inventory.Comments
                .OrderBy(c => c.CreatedAt)
                .Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    InventoryId = c.InventoryId,
                    AuthorName = c.Author.DisplayName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).ToList()
        };

        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var inventory = await _db.Inventories
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Forbid();

        ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");

        var model = new InventoryViewModel
        {
            Id = inventory.Id,
            Title = inventory.Title,
            Description = inventory.Description,
            IsPublic = inventory.IsPublic,
            CategoryId = inventory.CategoryId,
            ImageUrl = inventory.ImageUrl,
            RowVersion = inventory.RowVersion
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(InventoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
            return View(model);
        }

        var inventory = await _db.Inventories.FindAsync(model.Id);
        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Forbid();

        if (inventory.RowVersion != model.RowVersion)
        {
            ModelState.AddModelError(string.Empty, "This inventory was modified by someone else. Please reload and try again.");
            ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
            return View(model);
        }

        inventory.Title = model.Title;
        inventory.Description = model.Description;
        inventory.IsPublic = model.IsPublic;
        inventory.CategoryId = model.CategoryId;
        inventory.ImageUrl = model.ImageUrl;
        inventory.RowVersion += 1;
        inventory.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = inventory.Id });
    }

    [Authorize]
    public async Task<IActionResult> Fields(int id)
    {
        var inventory = await _db.Inventories
            .Include(i => i.Fields)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Forbid();

        var fields = inventory.Fields
            .OrderBy(f => f.SortOrder)
            .Select(f => new InventoryFieldViewModel
            {
                Id = f.Id,
                InventoryId = f.InventoryId,
                Title = f.Title,
                Description = f.Description,
                Type = f.Type,
                ShowInTable = f.ShowInTable,
                SortOrder = f.SortOrder,
                SlotIndex = f.SlotIndex
            })
            .ToList();

        ViewBag.InventoryId = id;
        ViewBag.InventoryTitle = inventory.Title;
        return View(fields);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddField(InventoryFieldViewModel model)
    {
        var inventory = await _db.Inventories
            .Include(i => i.Fields)
            .FirstOrDefaultAsync(i => i.Id == model.InventoryId);

        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Forbid();

        var existingSlots = inventory.Fields
            .Where(f => f.Type == model.Type)
            .Select(f => f.SlotIndex)
            .ToList();

        var nextSlot = Enumerable.Range(1, 3)
            .FirstOrDefault(s => !existingSlots.Contains(s));

        if (nextSlot == 0)
        {
            TempData["Error"] = "Maximum 3 fields of this type allowed.";
            return RedirectToAction(nameof(Fields), new { id = model.InventoryId });
        }

        var field = new InventoryField
        {
            InventoryId = model.InventoryId,
            Title = model.Title,
            Description = model.Description,
            Type = model.Type,
            ShowInTable = model.ShowInTable,
            SlotIndex = nextSlot,
            SortOrder = inventory.Fields.Count + 1
        };

        _db.InventoryFields.Add(field);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Fields), new { id = model.InventoryId });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteField(int fieldId, int inventoryId)
    {
        var field = await _db.InventoryFields.FindAsync(fieldId);
        if (field != null)
        {
            _db.InventoryFields.Remove(field);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Fields), new { id = inventoryId });
    }

    [Authorize]
    public async Task<IActionResult> Access(int id)
    {
        var inventory = await _db.Inventories
            .Include(i => i.AccessList)
                .ThenInclude(a => a.User)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Forbid();

        var model = new AccessViewModel
        {
            InventoryId = inventory.Id,
            InventoryTitle = inventory.Title,
            IsPublic = inventory.IsPublic,
            UsersWithAccess = inventory.AccessList.Select(a => new UserViewModel
            {
                Id = a.UserId,
                DisplayName = a.User.DisplayName,
                Email = a.User.Email ?? string.Empty
            }).ToList()
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddAccess(int inventoryId, string email)
    {
        var inventory = await _db.Inventories
            .FirstOrDefaultAsync(i => i.Id == inventoryId);

        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Forbid();

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            TempData["Error"] = "User not found.";
            return RedirectToAction(nameof(Access), new { id = inventoryId });
        }

        var exists = await _db.InventoryAccesses
            .AnyAsync(a => a.InventoryId == inventoryId && a.UserId == user.Id);

        if (!exists)
        {
            _db.InventoryAccesses.Add(new InventoryAccess
            {
                InventoryId = inventoryId,
                UserId = user.Id
            });
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Access), new { id = inventoryId });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RemoveAccess(int inventoryId, string userId)
    {
        var access = await _db.InventoryAccesses
            .FirstOrDefaultAsync(a => a.InventoryId == inventoryId && a.UserId == userId);

        if (access != null)
        {
            _db.InventoryAccesses.Remove(access);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Access), new { id = inventoryId });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> TogglePublic(int inventoryId)
    {
        var inventory = await _db.Inventories.FindAsync(inventoryId);
        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Forbid();

        inventory.IsPublic = !inventory.IsPublic;
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Access), new { id = inventoryId });
    }

    public async Task<IActionResult> Statistics(int id)
    {
        var inventory = await _db.Inventories
            .Include(i => i.Fields)
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inventory == null)
            return NotFound();

        var model = new InventoryStatisticsViewModel
        {
            InventoryId = inventory.Id,
            InventoryTitle = inventory.Title,
            TotalItems = inventory.Items.Count
        };

        foreach (var field in inventory.Fields.Where(f => f.Type == FieldType.Numeric))
        {
            var values = field.SlotIndex switch
            {
                1 => inventory.Items.Where(i => i.Number1.HasValue).Select(i => i.Number1!.Value).ToList(),
                2 => inventory.Items.Where(i => i.Number2.HasValue).Select(i => i.Number2!.Value).ToList(),
                3 => inventory.Items.Where(i => i.Number3.HasValue).Select(i => i.Number3!.Value).ToList(),
                _ => []
            };

            if (values.Count > 0)
            {
                model.NumericStats.Add(new NumericFieldStat
                {
                    FieldTitle = field.Title,
                    Average = values.Average(),
                    Min = values.Min(),
                    Max = values.Max()
                });
            }
        }

        foreach (var field in inventory.Fields.Where(f => f.Type == FieldType.SingleLineText))
        {
            var values = field.SlotIndex switch
            {
                1 => inventory.Items.Where(i => i.String1 != null).Select(i => i.String1!).ToList(),
                2 => inventory.Items.Where(i => i.String2 != null).Select(i => i.String2!).ToList(),
                3 => inventory.Items.Where(i => i.String3 != null).Select(i => i.String3!).ToList(),
                _ => []
            };

            if (values.Count > 0)
            {
                model.StringStats.Add(new StringFieldStat
                {
                    FieldTitle = field.Title,
                    TopValues = values
                        .GroupBy(v => v)
                        .OrderByDescending(g => g.Count())
                        .Take(5)
                        .Select(g => new ValueCount { Value = g.Key, Count = g.Count() })
                        .ToList()
                });
            }
        }

        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> CustomId(int id)
    {
        var inventory = await _db.Inventories
            .Include(i => i.CustomIdFormat)
                .ThenInclude(f => f!.Elements)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Forbid();

        var elements = inventory.CustomIdFormat?.Elements
            .OrderBy(e => e.SortOrder)
            .Select(e => new CustomIdElementViewModel
            {
                Id = e.Id,
                Type = e.Type,
                FixedValue = e.FixedValue,
                FormatPattern = e.FormatPattern,
                SortOrder = e.SortOrder
            })
            .ToList() ?? [];

        var model = new CustomIdViewModel
        {
            InventoryId = id,
            InventoryTitle = inventory.Title,
            Elements = elements,
            Preview = GeneratePreviewId(elements)
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddIdElement(int inventoryId, int type, string? fixedValue, string? formatPattern)
    {
        var inventory = await _db.Inventories
            .Include(i => i.CustomIdFormat)
                .ThenInclude(f => f!.Elements)
            .FirstOrDefaultAsync(i => i.Id == inventoryId);

        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Forbid();

        if (inventory.CustomIdFormat == null)
        {
            inventory.CustomIdFormat = new CustomIdFormat { InventoryId = inventoryId };
            _db.CustomIdFormats.Add(inventory.CustomIdFormat);
            await _db.SaveChangesAsync();
        }

        var elementCount = inventory.CustomIdFormat.Elements?.Count ?? 0;

        var element = new CustomIdElement
        {
            CustomIdFormatId = inventory.CustomIdFormat.Id,
            Type = (IdElementType)type,
            FixedValue = fixedValue,
            FormatPattern = formatPattern,
            SortOrder = elementCount + 1
        };

        _db.CustomIdElements.Add(element);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(CustomId), new { id = inventoryId });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteIdElement(int elementId, int inventoryId)
    {
        var element = await _db.CustomIdElements.FindAsync(elementId);
        if (element != null)
        {
            _db.CustomIdElements.Remove(element);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(CustomId), new { id = inventoryId });
    }

    private string GeneratePreviewId(List<CustomIdElementViewModel> elements)
    {
        if (elements.Count == 0)
            return "No format defined";

        var parts = elements.Select(e => e.Type switch
        {
            IdElementType.FixedText => e.FixedValue ?? string.Empty,
            IdElementType.Random20Bit => "A7E3B",
            IdElementType.Random32Bit => "A7E3B9F2",
            IdElementType.Random6Digit => "042857",
            IdElementType.Random9Digit => "042857391",
            IdElementType.Guid => "550e8400-e29b",
            IdElementType.DateTime => DateTime.UtcNow.ToString(e.FormatPattern ?? "yyyy"),
            IdElementType.Sequence => (1).ToString(e.FormatPattern ?? "D"),
            _ => string.Empty
        });

        return string.Concat(parts);
    }



    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var inventory = await _db.Inventories.FindAsync(id);
        if (inventory == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Forbid();

        _db.Inventories.Remove(inventory);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddTag(int inventoryId, string tagName)
    {
        if (string.IsNullOrWhiteSpace(tagName))
            return RedirectToAction(nameof(Details), new { id = inventoryId });

        tagName = tagName.Trim().ToLower();

        var tag = await _db.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
        if (tag == null)
        {
            tag = new Tag { Name = tagName };
            _db.Tags.Add(tag);
            await _db.SaveChangesAsync();
        }

        var exists = await _db.InventoryTags
            .AnyAsync(it => it.InventoryId == inventoryId && it.TagId == tag.Id);

        if (!exists)
        {
            _db.InventoryTags.Add(new InventoryTag
            {
                InventoryId = inventoryId,
                TagId = tag.Id
            });
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Details), new { id = inventoryId });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RemoveTag(int inventoryId, int tagId)
    {
        var inventoryTag = await _db.InventoryTags
            .FirstOrDefaultAsync(it => it.InventoryId == inventoryId && it.TagId == tagId);

        if (inventoryTag != null)
        {
            _db.InventoryTags.Remove(inventoryTag);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Details), new { id = inventoryId });
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AutoSave([FromBody] InventoryViewModel model)
    {
        var inventory = await _db.Inventories.FindAsync(model.Id);
        if (inventory == null)
            return Json(new { success = false, error = "Not found" });

        var userId = _userManager.GetUserId(User);
        if (inventory.OwnerId != userId)
            return Json(new { success = false, error = "Access denied" });

        if (inventory.RowVersion != model.RowVersion)
            return Json(new { success = false, error = "Modified by someone else. Please reload." });

        inventory.Title = model.Title;
        inventory.Description = model.Description;
        inventory.CategoryId = model.CategoryId;
        inventory.ImageUrl = model.ImageUrl;
        inventory.IsPublic = model.IsPublic;
        inventory.RowVersion += 1;
        inventory.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Json(new { success = true, rowVersion = inventory.RowVersion });
    }
}