using Tasks.Manager.Domain.DomainObjects.Enums;

namespace Tasks.Manager.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }

    public User(Guid id, string name, string email, UserRole role)
    {
        Id = id;
        Name = name;
        Email = email;
        Role = role;
    }
}
