using Tasks.Manager.Domain.DomainObjects.Enums;

namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.Update;

public class UpdateTaskItemRequest
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public TaskPriority Priority { get; set; } = default!;
    public TaskState Status { get; set; } = default!;
    public Guid? AssignedToUserId { get; set; }
}
