using System.ComponentModel.DataAnnotations;

namespace Tasks.Manager.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; protected set; }
    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; private set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }


    public void MarkUpdated() => UpdatedDate = DateTime.UtcNow;
}
