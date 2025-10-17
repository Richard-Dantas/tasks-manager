using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Admin.Application.UseCases.Project.Create;

public class CreateProjectUseCase(IProjectRepository _projectRepository) : ICreateProjectUseCase
{
    public async Task<Guid> ExecuteAsync(CreateProjectRequest request)
    {
        var project = new Domain.Entities.Project(request.Name, request.Description);
        project.AddMember(request.CreatorUserId);

        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();

        return project.Id;
    }
}
