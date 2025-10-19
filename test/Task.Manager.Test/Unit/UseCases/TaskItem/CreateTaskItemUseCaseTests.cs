using Moq;
using Tasks.Manager.Admin.Application.UseCases.TaskItem.Create;
using Tasks.Manager.Domain.DomainObjects.Enums;
using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Test.Unit.UseCases.TaskItem;

public class CreateTaskItemUseCaseTests
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly CreateTaskItemUseCase _useCase;

    public CreateTaskItemUseCaseTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _useCase = new CreateTaskItemUseCase(_mockProjectRepository.Object);
    }

    [Fact]
    public async Task Given_ValidRequest_When_ProjectExists_Then_ShouldAddTaskAndSaveChanges()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var assignedUserId = Guid.NewGuid();

        var project = new Domain.Entities.Project("Projeto Teste", "Descrição do projeto");

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var request = new CreateTaskItemRequest
        {
            ProjectId = projectId,
            Title = "Nova Tarefa",
            Description = "Descrição da tarefa",
            Priority = TaskPriority.Alta,
            AssignedToUserId = assignedUserId
        };

        // Act
        await _useCase.ExecuteAsync(request);

        // Assert
        Assert.Single(project.Tasks); 
        var task = project.Tasks.First();
        Assert.Equal(request.Title, task.Title);
        Assert.Equal(request.Description, task.Description);
        Assert.Equal(request.Priority, task.Priority);
        Assert.Equal(assignedUserId, task.AssignedToUserId);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
        _mockProjectRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_InvalidProjectId_When_ProjectNotFound_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var request = new CreateTaskItemRequest
        {
            ProjectId = Guid.NewGuid(),
            Title = "Tarefa inválida",
            Description = "Projeto não existe",
            Priority = TaskPriority.Baixa,
            AssignedToUserId = Guid.NewGuid()
        };

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(request.ProjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Project?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(request));
        Assert.Equal("Projeto não encontrado.", exception.Message);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(request.ProjectId, It.IsAny<CancellationToken>()), Times.Once);
        _mockProjectRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
