using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Admin.Application.UseCases.Project.Delete;

public class DeleteProjectUseCase(IProjectRepository _projectRepository) : IDeleteProjectUseCase
{
    public async Task ExecuteAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId)
            ?? throw new InvalidOperationException("Projeto não encontrado.");
       
        project.EnsureCanBeDeleted();

        _projectRepository.Remove(project);

        await _projectRepository.SaveChangesAsync();
    }
}
