namespace RaceDay.Api.Models;

public sealed class Result
{
    public string ResultID { get; set; } = Guid.NewGuid().ToString("D");
    public string EnrolmentID { get; set; } = "";
    public TimeSpan? FinishTime { get; set; }
    public int? Position { get; set; }
    public string ResultStatus { get; set; } = "Completed";
    public Enrolment? Enrolment { get; set; }
}
