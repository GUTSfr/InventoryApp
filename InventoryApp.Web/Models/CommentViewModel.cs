namespace InventoryApp.Web.Models;

public class CommentViewModel
{
    public int Id { get; set; }
    public int InventoryId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}   