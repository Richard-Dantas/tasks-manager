namespace Tasks.Manager.Domain.Entities;

public class TaskHistory
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TaskId { get; private set; }
    public Guid ModifiedByUserId { get; private set; }
    public string Changes { get; private set; } = default!;
    public DateTime ModifiedAt { get; private set; } = DateTime.UtcNow;

    public TaskHistory(Guid taskId, Guid modifiedByUserId, string changes)
    {
        TaskId = taskId;
        ModifiedByUserId = modifiedByUserId;
        Changes = changes;
    }
}
