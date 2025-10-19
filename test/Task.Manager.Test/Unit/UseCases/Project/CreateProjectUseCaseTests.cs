using Moq;
using Tasks.Manager.Admin.Application.UseCases.Project.Create;
using Tasks.Manager.Domain.Repositories;

namespace Tasks.Manager.Test.Unit.UseCases.Project;

public class CreateProjectUseCaseTests
{
    private readonly CreateProjectUseCase _useCase;
    private readonly Mock<IProjectRepository> _mockProjectRepository = new();

    public CreateProjectUseCaseTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _useCase = new CreateProjectUseCase(_mockProjectRepository.Object);
    }

    [Fact]
    public async Task Given_ValidRequest_When_Executed_Then_ShouldCreateProjectAndReturnId()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "Novo Projeto",
            Description = "Projeto de teste unitário",
            CreatorUserId = Guid.NewGuid()
        };

        Domain.Entities.Project? capturedProject = null;

        _mockProjectRepository
            .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Project>(), It.IsAny<CancellationToken>()))
            .Callback<Domain.Entities.Project, CancellationToken>((p, _) => capturedProject = p)
            .Returns(Task.CompletedTask);

        _mockProjectRepository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        Assert.NotNull(capturedProject);
        Assert.Equal(request.Name, capturedProject!.Name);
        Assert.Equal(request.Description, capturedProject.Description);
        Assert.Contains(capturedProject.Members, m => m.UserId == request.CreatorUserId);

        _mockProjectRepository.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Project>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockProjectRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
