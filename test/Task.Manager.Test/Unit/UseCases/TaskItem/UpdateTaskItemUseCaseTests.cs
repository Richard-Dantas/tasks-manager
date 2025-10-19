using Moq;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.Update;
using Tasks.Manager.Domain.DomainObjects.Enums;
using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Test.Unit.UseCases.TaskItem;

public class UpdateTaskItemUseCaseTests
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly UpdateTaskItemUseCase _useCase;

    public UpdateTaskItemUseCaseTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _useCase = new UpdateTaskItemUseCase(_mockProjectRepository.Object);
    }

    [Fact]
    public async Task Given_ValidProjectAndTask_When_Executed_Then_ShouldUpdateTaskAndSaveChanges()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var assignedUserId = Guid.NewGuid();
        var modifiedByUserId = Guid.NewGuid();

        var project = new Domain.Entities.Project("Projeto Teste", "Descrição");
        project.AddTask("Task original", "Descrição 1", TaskPriority.Alta, DateTime.UtcNow.AddDays(2), assignedUserId);
        var taskId = project.Tasks.FirstOrDefault()!.Id;

        var request = new UpdateTaskItemRequest
        {
            Title = "Título Atualizado",
            Description = "Descrição Atualizada",
            Status = TaskState.Concluida,
            AssignedToUserId = assignedUserId,
            Comment = null
        };

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        _mockProjectRepository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _useCase.ExecuteAsync(projectId, taskId, request, modifiedByUserId);

        // Assert
        var updatedTask = project.Tasks.First(t => t.Id == taskId);

        Assert.Equal(request.Title, updatedTask.Title);
        Assert.Equal(request.Description, updatedTask.Description);
        Assert.Equal(request.Status, updatedTask.Status);
        Assert.Equal(request.AssignedToUserId, updatedTask.AssignedToUserId);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
        _mockProjectRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_ValidUpdateWithComment_When_Executed_Then_ShouldAddCommentToTask()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var assignedUserId = Guid.NewGuid();
        var modifiedByUserId = Guid.NewGuid();

        var project = new Domain.Entities.Project("Projeto Teste", "Descrição");
        project.AddTask("Task original", "Descrição 1", TaskPriority.Alta, DateTime.UtcNow.AddDays(2), assignedUserId);
        var taskId = project.Tasks.FirstOrDefault()!.Id;

        var request = new UpdateTaskItemRequest
        {
            Title = "Título Atualizado",
            Description = "Descrição Atualizada",
            Status = TaskState.Em_Desenvolvimento,
            AssignedToUserId = assignedUserId,
            Comment = "Comentário de atualização"
        };

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        _mockProjectRepository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _useCase.ExecuteAsync(projectId, taskId, request, modifiedByUserId);

        // Assert
        var updatedTask = project.Tasks.First(t => t.Id == taskId);

        Assert.Single(updatedTask.Comments);
        var comment = updatedTask.Comments.First();
        Assert.Equal(request.Comment, comment.Content);
        Assert.Equal(modifiedByUserId, comment.UserId);

        _mockProjectRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_InvalidProjectId_When_ProjectNotFound_Then_ShouldThrowException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var modifiedByUserId = Guid.NewGuid();

        var request = new UpdateTaskItemRequest
        {
            Title = "Título",
            Description = "Descrição",
            Status = TaskState.Em_Desenvolvimento,
            AssignedToUserId = Guid.NewGuid()
        };

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Project?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.ExecuteAsync(projectId, taskId, request, modifiedByUserId));

        Assert.Equal("Projeto não encontrado.", exception.Message);

        _mockProjectRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Given_ValidProject_When_TaskDoesNotExist_Then_ShouldThrowException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var modifiedByUserId = Guid.NewGuid();

        var project = new Domain.Entities.Project("Projeto Teste", "Descrição");

        var request = new UpdateTaskItemRequest
        {
            Title = "Título",
            Description = "Descrição",
            Status = TaskState.Em_Desenvolvimento,
            AssignedToUserId = Guid.NewGuid()
        };

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.ExecuteAsync(projectId, taskId, request, modifiedByUserId));

        Assert.Contains("não encontrada", exception.Message, StringComparison.OrdinalIgnoreCase);

        _mockProjectRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

