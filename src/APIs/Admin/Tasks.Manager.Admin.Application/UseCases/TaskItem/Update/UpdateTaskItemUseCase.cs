using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.Update;

public class UpdateTaskItemUseCase(IProjectRepository _projectRepository) : IUpdateTaskItemUseCase
{
    public async Task ExecuteAsync(Guid projectId, Guid taskId, UpdateTaskItemRequest request)
    {
        var project = await _projectRepository.GetByIdAsync(projectId)
            ?? throw new InvalidOperationException("Projeto não encontrado.");

        project.UpdateTask(
            taskId,
            request.Title,
            request.Description,
            request.Priority,
            request.Status,
            request.AssignedToUserId
        );

        await _projectRepository.SaveChangesAsync();

        var updatedTask = project.Tasks.First(t => t.Id == taskId);
    }
}
