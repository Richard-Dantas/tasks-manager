namespace Tasks.Manager.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; private set; }

    public void MarkUpdated() => UpdatedDate = DateTime.UtcNow;
}
