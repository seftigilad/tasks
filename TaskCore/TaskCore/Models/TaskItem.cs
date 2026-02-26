namespace TaskCore.Models;

public class TaskItem
{
    public int Id { get; set; }
    public int TaskTypeId { get; set; }
    public int AssignedUserId { get; set; }
    public int CurrentStatus { get; set; }
    public bool IsOpen { get; set; }
    public string? CustomData { get; set; }

    public User AssignedUser { get; set; } = null!;
}
