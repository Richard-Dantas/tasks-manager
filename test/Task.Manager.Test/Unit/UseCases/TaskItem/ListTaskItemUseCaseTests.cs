using Moq;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.List;
using Tasks.Manager.Domain.DomainObjects.Enums;
using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Test.Unit.UseCases.TaskItem;

public class ListTaskItemUseCaseTests
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly ListTaskItemUseCase _useCase;

    public ListTaskItemUseCaseTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _useCase = new ListTaskItemUseCase(_mockProjectRepository.Object);
    }

    [Fact]
    public async Task Given_ProjectWithTasks_When_Executed_Then_ShouldReturnMappedTaskList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var assignedUserId = Guid.NewGuid();
        var commentAuthorId = Guid.NewGuid();

        var project = new Domain.Entities.Project("Projeto Teste", "Descrição");

        project.AddTask("Task 1", "Descrição 1", TaskPriority.Alta, DateTime.UtcNow.AddDays(2), assignedUserId);

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var result = await _useCase.ExecuteAsync(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        var firstTask = result.First();
        Assert.Equal("Task 1", firstTask.Title);
        Assert.Equal(TaskPriority.Alta, firstTask.Priority);
        Assert.Equal(assignedUserId, firstTask.AssignedToUserId);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_ProjectWithoutTasks_When_Executed_Then_ShouldReturnEmptyList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new Domain.Entities.Project("Projeto Vazio", "Sem tarefas");

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var result = await _useCase.ExecuteAsync(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_InvalidProjectId_When_ProjectNotFound_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Project?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(projectId));
        Assert.Equal("Projeto não encontrado.", exception.Message);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
