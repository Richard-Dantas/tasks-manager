using Tasks.Manager.Domain.Entities;

namespace Tasks.Manager.Domain.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Project project, CancellationToken cancellationToken = default);
    Task<List<Project>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<TaskItem>> GetCompletedTasksSinceAsync(DateTime since, CancellationToken cancellationToken = default);
    void Remove(Project project);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
