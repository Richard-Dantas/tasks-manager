using Microsoft.VisualBasic;
using Tasks.Manager.Domain.DomainObjects.Enums;

namespace Tasks.Manager.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskState Status { get; private set; } = TaskState.Pendente;
    public Guid? AssignedToUserId { get; private set; }
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; }

    public TaskItem(string title, string? description, Guid projectId,  Guid? assignedToUserId = null)
    {
        Title = title;
        Description = description;
        ProjectId = projectId;
        AssignedToUserId = assignedToUserId;
    }

    public void UpdateStatus(TaskState newStatus)
    {
        Status = newStatus;
    }
}
