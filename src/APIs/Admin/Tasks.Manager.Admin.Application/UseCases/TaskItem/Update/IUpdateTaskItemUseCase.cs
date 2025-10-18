namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.Update;

public interface IUpdateTaskItemUseCase
{
    Task ExecuteAsync(Guid projectId, Guid taskId, UpdateTaskItemRequest request, Guid modifiedByUserId);
}
