namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.List;

public interface IListTaskItemUseCase
{
    Task<List<ListTaskItemResponse>> ExecuteAsync(Guid id);
}
