namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.Create;

public class CreateTaskItemRequest
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? AssignedToUserId { get; set; }
}
