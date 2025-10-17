namespace Tasks.Manager.Admin.Application.UseCases.Project.Create;

public interface ICreateProjectUseCase
{
    Task<Guid> ExecuteAsync(CreateProjectRequest request);
}
