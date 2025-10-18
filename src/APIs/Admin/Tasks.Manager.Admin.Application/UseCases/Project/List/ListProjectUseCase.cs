using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Admin.Application.UseCases.Project.List;

public class ListProjectUseCase(IProjectRepository _projectRepository) : IListProjectUseCase
{
    public async Task<List<ListProjectResponse>> ExecuteAsync(Guid userId)
    {
        var projects = await _projectRepository.GetByUserIdAsync(userId);

        return projects.Select(p => new ListProjectResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            TaskCount = p.Tasks.Count,
            MemberCount = p.Members.Count
        }).ToList();
    }
}
