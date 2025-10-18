using Microsoft.EntityFrameworkCore;
using Tasks.Manager.Domain.DomainObjects.Enums;
using Tasks.Manager.Domain.Entities;
using Tasks.Manager.Domain.Repositories;
using Tasks.Manager.Infrastructure.Data;

namespace Tasks.Manager.Infrastructure.Persistence.SqlServer.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly DatabaseContext _context;
    private readonly DbSet<Project> _projects;

    public ProjectRepository(DatabaseContext context)
    {
        _context = context;
        _projects = _context.Set<Project>();
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _projects
            .Include(p => p.Tasks)
                .ThenInclude(t => t.History)
            .Include(p => p.Tasks)
                .ThenInclude(t => t.Comments)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        await _projects.AddAsync(project, cancellationToken);
    }

    public async Task<List<Project>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _projects
            .Include(p => p.Tasks)
            .Include(p => p.Members)
            .Where(p => p.Members.Any(m => m.UserId == userId))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TaskItem>> GetCompletedTasksSinceAsync(DateTime since, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .SelectMany(p => p.Tasks)
            .Where(t => t.Status == TaskState.Concluida && t.CompletedAt >= since)
            .ToListAsync(cancellationToken);
    }

    public void Remove(Project project)
    {
        _projects.Remove(project);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
