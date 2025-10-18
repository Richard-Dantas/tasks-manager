namespace Tasks.Manager.Admin.Application.UseCases.Project.List;

public class ListProjectResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int TaskCount { get; set; }
    public int MemberCount { get; set; }
}
