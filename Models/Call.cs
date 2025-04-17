namespace OTWebApiWithSql.Models;

public class Call
{
    public int Id { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string? Result { get; set; }
    public bool IsProcessed { get; set; }
    public List<CallAssignment> CallAssignments { get; set; } = [];
}