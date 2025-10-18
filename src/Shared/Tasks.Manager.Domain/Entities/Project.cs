using Tasks.Manager.Domain.DomainObjects.Enums;

namespace Tasks.Manager.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    private readonly List<TaskItem> _tasks = new();

    private readonly List<ProjectUser> _members = new();

    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    public IReadOnlyCollection<ProjectUser> Members => _members.AsReadOnly();

    public Project(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do projeto é obrigatório.", nameof(name));

        Name = name;
        Description = description;
    }

    public void AddTask(string title, string? description, TaskPriority priority, DateTime? dueDate, Guid? assignedToUserId = null)
    {
        if (_tasks.Count >= 20)
            throw new InvalidOperationException($"O projeto já possui o número máximo de 20 tarefas.");

        var task = new TaskItem(title, description, priority, Id, assignedToUserId);
        _tasks.Add(task);
    }

    public void UpdateTask(
    Guid taskId,
    string title,
    string description,
    TaskPriority priority,
    TaskState status,
    Guid? assignedToUserId)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == taskId)
            ?? throw new InvalidOperationException("Tarefa não encontrada neste projeto.");

        task.Update(title, description, priority, status, assignedToUserId);
    }

    public void AddMember(Guid userId)
    {
        if (_members.Any(m => m.UserId == userId))
            throw new InvalidOperationException("Usuário já faz parte do projeto.");

        _members.Add(new ProjectUser(userId));
    }

    public void RemoveTask(Guid taskId)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null)
            throw new InvalidOperationException("Tarefa não encontrada.");

        _tasks.Remove(task);
    }

    public void EnsureCanBeDeleted()
    {
        bool hasPendingTasks = _tasks.Any(t => t.Status == TaskState.Pendente);

        if (hasPendingTasks)
            throw new InvalidOperationException(
                "O projeto não pode ser removido enquanto houver tarefas pendentes. " +
                "Conclua ou remova as tarefas antes de prosseguir.");
    }
}
