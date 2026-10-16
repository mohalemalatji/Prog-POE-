namespace RaceDay.Api.Models;

public sealed class Event
{
    public string EventID { get; set; } = Guid.NewGuid().ToString("D");
    public string OrganiserID { get; set; } = "";
    public string EventName { get; set; } = "";
    public string? Description { get; set; }
    public DateOnly EventDate { get; set; }
    public string Location { get; set; } = "";
    public decimal DistanceKm { get; set; }
    public string EventType { get; set; } = "Run";
    public string Status { get; set; } = "Upcoming";
    public User? Organiser { get; set; }
    public ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();
}
