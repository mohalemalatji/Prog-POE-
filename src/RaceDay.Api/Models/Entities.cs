namespace RaceDay.Api.Models;

public sealed class EventCategory
{
    public string EventCategoryID { get; set; } = Guid.NewGuid().ToString("D");
    public string EventID { get; set; } = "";
    public string CategoryID { get; set; } = "";
    public decimal EntryFee { get; set; }
    public int MaxParticipants { get; set; }
    public Event? Event { get; set; }
    public Category? Category { get; set; }
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
