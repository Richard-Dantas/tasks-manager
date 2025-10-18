using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.Remove;

public class RemoveTaskItemUseCase(IProjectRepository _projectRepository) : IRemoveTaskItemUseCase
{
    public async Task ExecuteAsync(Guid projectId, Guid taskId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId)
            ?? throw new InvalidOperationException("Projeto não encontrado.");

        var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null)
            throw new InvalidOperationException("Tarefa não encontrada no projeto.");

        project.RemoveTask(taskId);

        await _projectRepository.SaveChangesAsync();
    }
}
