using Tasks.Manager.Domain.DomainObjects.Enums;
using Tasks.Manager.Domain.Entities;

namespace Tasks.Manager.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static void Seed(this DatabaseContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Alice", "alice@example.com", UserRole.Desenvolvedor),
                        new User(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Bob", "bob@example.com", UserRole.Desenvolvedor),
                        new User(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Carol", "carol@example.com", UserRole.Gerente)
            );

            context.SaveChanges();
        }
    }
}