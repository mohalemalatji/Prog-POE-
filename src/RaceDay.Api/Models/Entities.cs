namespace RaceDay.Api.Models;

public sealed class Category
{
    public string CategoryID { get; set; } = Guid.NewGuid().ToString("D");
    public string CategoryName { get; set; } = "";
    public string? Description { get; set; }
    public ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();
}
