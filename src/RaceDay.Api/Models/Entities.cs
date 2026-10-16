namespace RaceDay.Api.Models;

public sealed class User
{
    public string UserID { get; set; } = Guid.NewGuid().ToString("D");
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "Participant";
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
