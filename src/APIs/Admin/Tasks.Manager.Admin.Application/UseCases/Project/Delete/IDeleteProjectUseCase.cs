namespace Tasks.Manager.Admin.Application.UseCases.Project.Delete;

public interface IDeleteProjectUseCase
{
    Task ExecuteAsync(Guid projectId);
}
