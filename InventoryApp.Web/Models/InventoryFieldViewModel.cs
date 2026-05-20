using System.ComponentModel.DataAnnotations;
using InventoryApp.Core.Entities;

namespace InventoryApp.Web.Models;

public class InventoryFieldViewModel
{
    public int Id { get; set; }
    public int InventoryId { get; set; }

    [Required]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Type")]
    public FieldType Type { get; set; }

    [Display(Name = "Show in table")]
    public bool ShowInTable { get; set; }

    public int SortOrder { get; set; }
    public int SlotIndex { get; set; }
}