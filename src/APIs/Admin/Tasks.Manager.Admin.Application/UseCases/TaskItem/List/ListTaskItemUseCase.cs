using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Admin.Application.UseCases.TaskItem.List;

public class ListTaskItemUseCase(IProjectRepository _projectRepository) : IListTaskItemUseCase
{
    public async Task<List<ListTaskItemResponse>> ExecuteAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Projeto não encontrado.");

        List<ListTaskItemResponse> taskItems = project.Tasks.Select(t => new ListTaskItemResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Priority = t.Priority,
            Status = t.Status,
            AssignedToUserId = t.AssignedToUserId,
            Comments = [.. t.Comments.Select(c => new TaskCommentResponse
            {
                Id = c.Id,
                Content = c.Content,
                AuthorUserId = c.UserId,
                CreatedAt = c.CreatedDate
            })]
        }).ToList();

        return taskItems;
    }
}
