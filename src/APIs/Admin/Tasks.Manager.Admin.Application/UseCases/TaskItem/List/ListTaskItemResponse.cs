using Tasks.Manager.Domain.DomainObjects.Enums;

namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.List;

public class ListTaskItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskState Status { get; set; } = TaskState.Pendente;
    public TaskPriority Priority { get; set; } = TaskPriority.Baixa;
    public Guid? AssignedToUserId { get; set; }
}
