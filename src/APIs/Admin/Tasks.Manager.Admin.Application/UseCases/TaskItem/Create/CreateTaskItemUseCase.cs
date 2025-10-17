using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.Create;

public class CreateTaskItemUseCase(IProjectRepository _projectRepository) : ICreateTaskItemUseCase
{
    public async Task ExecuteAsync(CreateTaskItemRequest request)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId)
            ?? throw new InvalidOperationException("Projeto não encontrado.");

        project.AddTask(request.Title, request.Description, DateTime.UtcNow.AddDays(3), request.AssignedToUserId);

        await _projectRepository.SaveChangesAsync();
    }
}
