using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Web.Models;

public class InventoryViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Public")]
    public bool IsPublic { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? OwnerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ItemCount { get; set; }
    public int RowVersion { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<TagViewModel> TagsWithId { get; set; } = [];
    public List<ItemViewModel> Items { get; set; } = [];
    public List<CommentViewModel> Comments { get; set; } = [];
}

public class TagViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}