namespace RaceDay.Api.Models;

public sealed class Enrolment
{
    public string EnrolmentID { get; set; } = Guid.NewGuid().ToString("D");
    public string ParticipantID { get; set; } = "";
    public string EventCategoryID { get; set; } = "";
    public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Registered";
    public User? Participant { get; set; }
    public EventCategory? EventCategory { get; set; }
    public Result? Result { get; set; }
}
