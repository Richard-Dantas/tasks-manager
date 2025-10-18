using Microsoft.EntityFrameworkCore;
using Tasks.Manager.Domain.Entities;
using Tasks.Manager.Domain.Repositories;
using Tasks.Manager.Infrastructure.Data;

namespace Tasks.Manager.Infrastructure.Persistence.SqlServer.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;
    private readonly DbSet<User> _users;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
        _users = _context.Set<User>();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
