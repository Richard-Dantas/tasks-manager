using Moq;
using Tasks.Manager.Admin.Application.UseCases.Report.Get;
using Tasks.Manager.Domain.DomainObjects.Enums;
using Tasks.Manager.Domain.Entities;
using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Test.Unit.UseCases.Report;

public class GetReportUseCaseTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetReportUseCase _sut; // System Under Test

    public GetReportUseCaseTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _sut = new GetReportUseCase(_projectRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync((User)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _sut.ExecuteAsync(userId));

        Assert.Equal("Usuário não encontrado.", ex.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrow_WhenUserIsNotManager()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User(userId, "user teste", "user@mail.com", UserRole.Desenvolvedor);

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(user);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _sut.ExecuteAsync(userId));

        Assert.Equal("Acesso negado. Apenas gerentes podem gerar relatórios.", ex.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnGroupedReport_WhenUserIsManager()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var gerente = new User(userId, "user teste", "user@mail.com", UserRole.Gerente);

        var project = new Domain.Entities.Project("Projeto Teste", "Descrição teste");

        var userA = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var userB = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        project.AddTask("Task A1", "Descrição A1", TaskPriority.Alta, DateTime.UtcNow, userA);
        project.AddTask("Task A2", "Descrição A2", TaskPriority.Baixa, DateTime.UtcNow, userA);
        project.AddTask("Task B1", "Descrição B1", TaskPriority.Media, DateTime.UtcNow, userB);
        project.AddTask("Task C1", "Descrição C1", TaskPriority.Media, DateTime.UtcNow, null);

        foreach (var task in project.Tasks.Where(t => t.AssignedToUserId != null))
        {
            task.Update(
                task.Title,
                task.Description,
                task.Priority,
                TaskState.Concluida,
                task.AssignedToUserId,
                userId
            );
        }

        var completedTasks = project.Tasks
            .Where(t => t.Status == TaskState.Concluida)
            .ToList();

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(gerente);

        _projectRepositoryMock
            .Setup(r => r.GetCompletedTasksSinceAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(completedTasks);

        // Act
        var result = await _sut.ExecuteAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count); // Apenas dois usuários com AssignedToUserId

        var firstUserReport = result.First(r => r.UserId == userA);
        Assert.Equal(2, firstUserReport.CompletedTasks);
        Assert.Equal(Math.Round(2 / 30.0, 2), firstUserReport.AverageTasksPerDay);

        var secondUserReport = result.First(r => r.UserId == userB);
        Assert.Equal(1, secondUserReport.CompletedTasks);
        Assert.Equal(Math.Round(1 / 30.0, 2), secondUserReport.AverageTasksPerDay);

        _userRepositoryMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
        _projectRepositoryMock.Verify(r => r.GetCompletedTasksSinceAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnEmptyList_WhenNoCompletedTasks()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var gerente = new User(userId, "user teste", "user@mail.com", UserRole.Gerente);

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(gerente);

        _projectRepositoryMock
            .Setup(r => r.GetCompletedTasksSinceAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Entities.TaskItem>());

        // Act
        var result = await _sut.ExecuteAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _userRepositoryMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
        _projectRepositoryMock.Verify(r => r.GetCompletedTasksSinceAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
