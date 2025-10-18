using Microsoft.EntityFrameworkCore;
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
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        await _projects.AddAsync(project, cancellationToken);
    }

    public void Remove(Project project)
    {
        _projects.Remove(project);
    }

    public async Task<List<Project>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _projects
            .Include(p => p.Tasks)
            .Include(p => p.Members)
            .Where(p => p.Members.Any(m => m.UserId == userId))
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
