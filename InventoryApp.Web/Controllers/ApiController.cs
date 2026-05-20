using InventoryApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Web.Controllers;

[Route("api")]
public class ApiController : Controller
{
    private readonly AppDbContext _db;

    public ApiController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("tags")]
    public async Task<IActionResult> GetTags(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Json(new List<string>());

        var tags = await _db.Tags
            .Where(t => t.Name.StartsWith(query.ToLower()))
            .Select(t => t.Name)
            .Take(10)
            .ToListAsync();

        return Json(tags);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Json(new List<object>());

        var users = await _db.Users
            .Where(u => u.Email!.Contains(query) || u.DisplayName.Contains(query))
            .Select(u => new { u.Email, u.DisplayName })
            .Take(10)
            .ToListAsync();

        return Json(users);
    }
}