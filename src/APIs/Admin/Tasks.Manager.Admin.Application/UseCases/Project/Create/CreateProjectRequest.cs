namespace Tasks.Manager.Admin.Application.UseCases.Project.Create;

public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid CreatorUserId { get; set; }
}
