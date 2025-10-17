namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.Create;

public interface ICreateTaskItemUseCase
{
    Task ExecuteAsync(CreateTaskItemRequest request);
}
