using Moq;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.Remove;
using Tasks.Manager.Domain.DomainObjects.Enums;
using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Test.Unit.UseCases.TaskItem;

public class RemoveTaskItemUseCaseTests
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly RemoveTaskItemUseCase _useCase;

    public RemoveTaskItemUseCaseTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _useCase = new RemoveTaskItemUseCase(_mockProjectRepository.Object);
    }

    [Fact]
    public async Task Given_ValidProjectAndTask_When_Executed_Then_ShouldRemoveTaskAndSaveChanges()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var assignedUserId = Guid.NewGuid();

        var project = new Domain.Entities.Project("Projeto Teste", "Descrição Teste");

        project.AddTask("Task 1", "Descrição 1", TaskPriority.Alta, DateTime.UtcNow.AddDays(2), assignedUserId);
        var taskId = project.Tasks.FirstOrDefault()!.Id;

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        _mockProjectRepository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _useCase.ExecuteAsync(projectId, taskId);

        // Assert
        Assert.DoesNotContain(project.Tasks, t => t.Id == taskId);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
        _mockProjectRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_InvalidProjectId_When_ProjectNotFound_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Project?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.ExecuteAsync(projectId, taskId));

        Assert.Equal("Projeto não encontrado.", exception.Message);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId , It.IsAny<CancellationToken>()), Times.Once);
        _mockProjectRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Given_ValidProject_When_TaskNotFound_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var project = new Domain.Entities.Project("Projeto Teste", "Descrição Teste");

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.ExecuteAsync(projectId, taskId));

        Assert.Equal("Tarefa não encontrada no projeto.", exception.Message);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
        _mockProjectRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
