using Tasks.Manager.Domain.Entities;

namespace Tasks.Manager.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
}
