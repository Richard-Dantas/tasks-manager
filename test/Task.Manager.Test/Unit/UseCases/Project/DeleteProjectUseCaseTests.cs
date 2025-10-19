using Moq;
using Tasks.Manager.Admin.Application.UseCases.Project.Delete;
using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Test.Unit.UseCases.Project;

public class DeleteProjectUseCaseTests
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly DeleteProjectUseCase _useCase;

    public DeleteProjectUseCaseTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _useCase = new DeleteProjectUseCase(_mockProjectRepository.Object);
    }

    [Fact]
    public async Task Given_ExistingProject_When_Executed_Then_ShouldDeleteProjectSuccessfully()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new Domain.Entities.Project("Projeto de Teste", "Descrição do projeto");

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, default))
            .ReturnsAsync(project);

        _mockProjectRepository
            .Setup(r => r.SaveChangesAsync(default))
            .Returns(Task.CompletedTask);

        // Act
        await _useCase.ExecuteAsync(projectId);

        // Assert
        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, default), Times.Once);
        _mockProjectRepository.Verify(r => r.Remove(project), Times.Once);
        _mockProjectRepository.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Given_NonExistingProject_When_Executed_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        _mockProjectRepository
            .Setup(r => r.GetByIdAsync(projectId, default))
            .ReturnsAsync((Domain.Entities.Project?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(projectId));

        _mockProjectRepository.Verify(r => r.Remove(It.IsAny<Domain.Entities.Project>()), Times.Never);
        _mockProjectRepository.Verify(r => r.SaveChangesAsync(default), Times.Never);
    }
}
