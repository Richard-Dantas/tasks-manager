namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.Remove;

public interface IRemoveTaskItemUseCase
{
    Task ExecuteAsync(Guid projectId, Guid taskId);
}
