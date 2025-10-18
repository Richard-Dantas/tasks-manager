using System.Threading;
using Tasks.Manager.Domain.DomainObjects.Enums;
using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Admin.Application.UseCases.Report.Get;

public class GetReportUseCase(
    IProjectRepository _projectRepository,
    IUserRepository _userRepository) : IGetReportUseCase
{
    public async Task<List<GetReportResponse>> ExecuteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("Usuário não encontrado.");

        if (user.Role != UserRole.Gerente)
            throw new UnauthorizedAccessException("Acesso negado. Apenas gerentes podem gerar relatórios.");

        var since = DateTime.UtcNow.AddDays(-30);
        var completedTasks = await _projectRepository.GetCompletedTasksSinceAsync(since, cancellationToken);

        var grouped = completedTasks
            .Where(t => t.AssignedToUserId != null)
            .GroupBy(t => t.AssignedToUserId!.Value)
            .Select(g => new GetReportResponse
            {
                UserId = g.Key,
                CompletedTasks = g.Count(),
                AverageTasksPerDay = Math.Round(g.Count() / 30.0, 2)
            })
            .ToList();

        return grouped;
    }
}
