namespace Tasks.Manager.Domain.Entities;

public class ProjectUser
{
    public Guid ProjectId { get; private set; }
    public Guid UserId { get; private set; }

    public Project? Project { get; private set; }
    public User? User { get; private set; }

    public ProjectUser(Guid userId)
    {
        UserId = userId;
    }
}
