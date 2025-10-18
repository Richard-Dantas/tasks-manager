namespace Tasks.Manager.Admin.Application.UseCases.Project.List;

public interface IListProjectUseCase
{
    Task<List<ListProjectResponse>> ExecuteAsync(Guid userId);
}
