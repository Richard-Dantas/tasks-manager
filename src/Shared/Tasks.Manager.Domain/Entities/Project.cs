namespace Tasks.Manager.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    private readonly List<TaskItem> _tasks = new();

    private readonly List<ProjectUser> _members = new();

    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    public IReadOnlyCollection<ProjectUser> Members => _members.AsReadOnly();

    public void AddTask(string title, string? description, DateTime? dueDate, Guid? assignedToUserId = null)
    {
        var task = new TaskItem(title, description, assignedToUserId);
        _tasks.Add(task);
    }

    public void AddMember(Guid userId)
    {
        if (_members.Any(m => m.UserId == userId))
            throw new InvalidOperationException("Usuário já faz parte do projeto.");

        _members.Add(new ProjectUser(userId));
    }
}
