using System.Text.Json;
using Tasks.Manager.Domain.DomainObjects.Enums;

namespace Tasks.Manager.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskState Status { get; private set; } = TaskState.Pendente;
    public TaskPriority Priority { get; private set; } = TaskPriority.Baixa;
    public DateTime? DueDate { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public Guid? AssignedToUserId { get; private set; }
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; }

    private readonly List<TaskHistory> _history = new();
    private readonly List<TaskComment> _comments = new();
    public IReadOnlyCollection<TaskHistory> History => _history.AsReadOnly();
    public IReadOnlyCollection<TaskComment> Comments => _comments.AsReadOnly();

    public TaskItem(string title, string? description, TaskPriority priority, Guid projectId, Guid? assignedToUserId = null)
    {
        Title = title;
        Description = description;
        Priority = priority;
        ProjectId = projectId;
        AssignedToUserId = assignedToUserId;
    }

    public void Update(string title, string description, TaskState status, Guid? assignedToUserId, Guid modifiedByUserId)
    {
        var changes = new Dictionary<string, object>();

        if (title != Title)
        {
            changes[nameof(Title)] = new { Old = Title, New = title };
            Title = title;
        }

        if (description != Description)
        {
            changes[nameof(Description)] = new { Old = Description, New = description };
            Description = description;
        }

        if (status != Status)
        {
            changes[nameof(Status)] = new { Old = Status.ToString(), New = status.ToString() };
            Status = status;

            if (status == TaskState.Concluida) 
            { 
                CompletedAt = DateTime.Now;
            }
        }

        if (assignedToUserId != AssignedToUserId)
        {
            changes[nameof(AssignedToUserId)] = new { Old = AssignedToUserId, New = assignedToUserId };
            AssignedToUserId = assignedToUserId;
        }

        if (changes.Count > 0)
        {
            var json = JsonSerializer.Serialize(changes);
            _history.Add(new TaskHistory(Id, modifiedByUserId, json));
        }
    }

    public void AddComment(string content, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Comentário não pode ser vazio.", nameof(content));

        var comment = new TaskComment(Id, userId, content);
        _comments.Add(comment);

        var change = new { Comment = content };
        var json = JsonSerializer.Serialize(change);
        _history.Add(new TaskHistory(Id, userId, json));
    }
}
