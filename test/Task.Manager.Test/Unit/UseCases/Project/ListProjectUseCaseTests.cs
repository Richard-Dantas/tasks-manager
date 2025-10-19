using Moq;
using Tasks.Manager.Admin.Application.UseCases.Project.List;
using Tasks.Manager.Domain.DomainObjects.Enums;
using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Test.Unit.UseCases.Project;

public class ListProjectUseCaseTests
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly ListProjectUseCase _useCase;

    public ListProjectUseCaseTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _useCase = new ListProjectUseCase(_mockProjectRepository.Object);
    }

    [Fact]
    public async Task Given_UserWithProjects_When_Executed_Then_ShouldReturnMappedProjectList()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var project1 = new Domain.Entities.Project("Projeto 1", "Descrição 1");
        var project2 = new Domain.Entities.Project("Projeto 2", "Descrição 2");

        project1.AddMember(userId);
        project2.AddMember(userId);

        project1.AddTask("Task 1", "Descrição task 1", TaskPriority.Alta, DateTime.UtcNow, userId);
        project2.AddTask("Task 2", "Descrição task 2", TaskPriority.Baixa, DateTime.UtcNow, userId);

        var projects = new List<Domain.Entities.Project> { project1, project2 };

        _mockProjectRepository
                .Setup(r => r.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projects);

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, r => Assert.NotEqual(Guid.Empty, r.Id));
        Assert.Equal(project1.Name, result.First().Name);
        Assert.Equal(1, result.First().TaskCount);
        Assert.Equal(1, result.First().MemberCount);

        _mockProjectRepository.Verify(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Given_UserWithoutProjects_When_Executed_Then_ShouldReturnEmptyList()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mockProjectRepository
            .Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Entities.Project>());

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _mockProjectRepository.Verify(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
