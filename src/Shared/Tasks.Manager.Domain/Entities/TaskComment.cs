namespace Tasks.Manager.Domain.Entities;

public class TaskComment : BaseEntity
{
    public Guid TaskId { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; } = default!;

    public TaskComment(Guid taskId, Guid userId, string content)
    {
        TaskId = taskId;
        UserId = userId;
        Content = content;
    }
}